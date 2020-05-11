using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour , IChargable
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



    private void FixedUpdate()
    {
        time += 0.02f;
        transform.localScale = Vector3.one * sizeOverTime.Evaluate(time/lifeTime);
        transform.position += transform.forward * speedOverTime.Evaluate(time) * speed * Time.fixedDeltaTime;
    }
    public void SetCharge(float charge)
    {
        Init(charge);
    }
    public void Init(float charge = 0)
    {
        lifeTime = lifeTimeOverCharge.Evaluate(charge);
        speed = speedOverCharge.Evaluate(charge);
        damageData.damage *= dmgMultOverCharge.Evaluate(charge);
        damageData.specialEffectDmgPerSec *= dmgMultOverCharge.Evaluate(charge);
        Invoke("End", lifeTime);
        
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
        if (other.collider.CompareTag("Enemy"))
        {
            onHit.Invoke();
            other.collider.SendMessage("OnDamaged", damageData, SendMessageOptions.DontRequireReceiver);
        }
        onHit.Invoke();
        transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, other.GetContact(0).normal));
        Debug.Log(Vector3.Reflect(transform.forward, other.GetContact(0).normal));
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
}
[System.Serializable]
public struct DamageData
{
    public float damage;
    public SpecialEffect specialEffect;
    public float specialEffectDurationInSec;
    public float specialEffectDmgPerSec;
    public ElementType element;
}
interface IChargable
{
    void SetCharge(float charge);
}
