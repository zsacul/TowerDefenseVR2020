using System.Collections;
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


    private float lifeTime;
    private float time;
    private float speed;
    public UnityEvent onInit;
    public UnityEvent onHit;
    public UnityEvent onEnd;

    private bool released;
    private Vector3 lastPos;
    private Vector3 movement;
    private void FixedUpdate()
    {
        if(released)
        {
            time += 0.02f;
            transform.localScale = Vector3.one * sizeOverTime.Evaluate(time / lifeTime);
            //transform.position += transform.forward * speedOverTime.Evaluate(time) * speed * Time.fixedDeltaTime;
        }
        else
        {
            
        }
    }
    public void SetCharge(float charge)
    {
        Init(charge);
    }
    public void Init(float charge = 0)
    {
        GetComponent<Collider>().enabled = false;
        lifeTime = lifeTimeOverCharge.Evaluate(charge);
        speed = speedOverCharge.Evaluate(charge);
        damageData.damage *= dmgMultOverCharge.Evaluate(charge);
        damageData.specialEffectDmgPerSec *= dmgMultOverCharge.Evaluate(charge);
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
                onHit.Invoke();
                other.collider.BroadcastMessage("ApplyDamage", damageData, SendMessageOptions.DontRequireReceiver);
            }
            onHit.Invoke();
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
        released = true;
        transform.parent = null;
        Invoke("EnableCollision", 0.25f);
        Invoke("End", lifeTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = transform.forward * 15 * speed;
        GetComponent<LineRenderer>().enabled = false;
    }
}
