using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BulletNoTarget : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float damage;
    [SerializeField]
    float beginForce;
    int specialEffectDurationInSec;
    int specialEffectDmgPerSec;
    SpecialEffect specialEffect;
    [SerializeField]
    ElementType type;
    bool readyToDestroy;
    Rigidbody arrowRB;
    public UnityEvent EnemyHit;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    private void Start()
    {
        arrowRB = GetComponentInChildren<Rigidbody>();
        arrowRB.AddForce(transform.forward * -1f * beginForce, ForceMode.Impulse);
    }

    public void ChangeSpecialEffect(SpecialEffect SE)
    {
        specialEffect = SE;
    }

    void FixedUpdate()
    {
        if (readyToDestroy)
            destroyBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.BroadcastMessage("ApplyDamage", damage);
            EnemyHit.Invoke();
        }
        else if (collision.gameObject.tag != "Bullet")
            readyToDestroy = true;
    }

    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void SetReadyToDestroy()
    {
        readyToDestroy = true;
    }

    public float GetDamage()
    {
        return damage;
    }

    public ElementType GetBulletType()
    {
        return type;
    }

    public int GetSpecialEffectDuration()
    {
        return specialEffectDurationInSec;
    }

    public int GetSpecialEffectDmg()
    {
        return specialEffectDmgPerSec;
    }

    public SpecialEffect GetSpecialEffect()
    {
        return specialEffect;
    }
}
