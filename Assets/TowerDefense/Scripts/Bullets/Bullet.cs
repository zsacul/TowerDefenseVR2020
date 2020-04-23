using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    float speed;
    float damage;
    int specialEffectDurationInSec;
    int specialEffectDmgPerSec;
    SpecialEffect specialEffect;
    ElementType type;
    bool readyToDestroy;

    public Bullet(GameObject t, float s, float d, ElementType typ, int eDur = 0, int eDmg = 0, SpecialEffect specialEffect = SpecialEffect.none)
    {
        setBulletInfo(t, s, d, typ, eDur, eDmg);
    }

    public void setBulletInfo(GameObject t, float s, float d, ElementType typ, int eDur = 0, int eDmg = 0, SpecialEffect SE = SpecialEffect.none)
    {
        target = t;
        speed = s;
        damage = d;
        specialEffectDurationInSec = eDur;
        specialEffectDmgPerSec = eDmg;
        type = typ;
        specialEffect = SE;
        readyToDestroy = false;
    } 

    public void ChangeSpecialEffect(SpecialEffect SE)
    {
        specialEffect = SE;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        transform.LookAt(target.transform.position);
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
