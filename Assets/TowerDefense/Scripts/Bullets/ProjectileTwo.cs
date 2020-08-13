﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileTwo : MonoBehaviour , IChargable
{
    public DamageData damageData;

    public AnimationCurve sizeOverTime = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve speedOverTime = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve dmgMultOverCharge = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve speedOverCharge = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve lifeTimeOverCharge = AnimationCurve.Constant(0, 1, 1);
    public AnimationCurve sizeOverCharge = AnimationCurve.Constant(0, 0, 1);


    private float lifeTime;
    private float time;
    private float speed;
    public UnityEvent onInit;
    public UnityEvent onHit;
    public UnityEvent onEnd;
    public UnityEvent onRelease;

    private float charge;
    private bool released;
    private Vector3 lastPos;
    private Vector3 movement;
    private float lastHit;

    private void FixedUpdate()
    {
        if(released)
        {
            time += 0.02f;
            transform.localScale = Vector3.one * sizeOverTime.Evaluate(time / lifeTime) * sizeOverCharge.Evaluate(charge);
            //transform.position += transform.forward * speedOverTime.Evaluate(time) * speed * Time.fixedDeltaTime;
        }
        else
        {
            
        }
    }
    public void SetCharge(float charge)
    {
        this.charge = charge;
        Init(charge);
    }
    public void Init(float charge = 0)
    {
        transform.localScale = Vector3.one * sizeOverTime.Evaluate(0) * sizeOverCharge.Evaluate(charge);
        GetComponent<Collider>().enabled = false;
        lifeTime = lifeTimeOverCharge.Evaluate(charge);
        speed = speedOverCharge.Evaluate(charge);
        onInit.Invoke();
    }
    private void End()
    {
        onEnd.Invoke();
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Renderer>());
        Destroy(this);
        Destroy(gameObject, 1);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(released)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.collider.BroadcastMessage("ApplyDamage", damageData, SendMessageOptions.DontRequireReceiver);
            }
            if(Time.time - lastHit > 0.05f)
            {
                lastHit = Time.time;
                onHit.Invoke();
            }
            //transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, other.GetContact(0).normal));
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Spawn(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
    public void AddLifeTime(float amount)
    {
        CancelInvoke("End");
        lifeTime += amount;
        Invoke("End", lifeTime - time);
    }
    void EnableCollision()
    {
        GetComponent<Collider>().enabled = true;
    }
    public void Release()
    {
        damageData.damage *= dmgMultOverCharge.Evaluate(charge);
        damageData.specialEffectDmgPerSec *= dmgMultOverCharge.Evaluate(charge);
        onRelease.Invoke();
        released = true;
        if(transform.parent != null) transform.parent = null;
        Invoke("EnableCollision", 0.25f);
        Invoke("End", lifeTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = transform.forward * 15 * speed;
        GetComponent<LineRenderer>().enabled = false;
    }
}
