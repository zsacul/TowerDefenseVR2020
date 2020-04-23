using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNoTarget : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float damage;
    int specialEffectDurationInSec;
    int specialEffectDmgPerSec;
    SpecialEffect specialEffect;
    [SerializeField]
    ElementType type;
    bool readyToDestroy;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    public void ChangeSpecialEffect(SpecialEffect SE)
    {
        specialEffect = SE;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
        if (readyToDestroy)
            destroyBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.BroadcastMessage("ApplyDamage", this);
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
