using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    [SerializeField]
    float speed;
    [SerializeField]
    float damage;
    int specialEffectDurationInSec;
    int specialEffectDmgPerSec;
    SpecialEffect specialEffect;
    [SerializeField]
    ElementType type;
    float FindEnemyRange;
    bool readyToDestroy;

    private void Start()
    {
        FindEnemyRange = 2f;
    }

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
        if (target == null)
        {
            if (!findInRange())
            {
                transform.position += transform.forward * step;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            transform.LookAt(target.transform.position);
        }
        
        if (readyToDestroy)
            destroyBullet();
    }

    private bool findInRange()
    {
        Collider[] cld = Physics.OverlapSphere(transform.position, FindEnemyRange);
        foreach(Collider c in cld)
        {
            if (c.GetComponent<EnemyHPManager>())
            {
                target = c.gameObject;
                return true;
            }
        }

        return false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.BroadcastMessage("ApplyDamage", this);
        }
        else if (other.gameObject.tag != "Bullet")
            readyToDestroy = true;
    }

    private void destroyBullet()
    {
        Destroy(GetComponent<Collider>());
        Destroy(this.gameObject, 0.5f);
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
