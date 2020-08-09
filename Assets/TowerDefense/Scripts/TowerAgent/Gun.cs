using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    public void SetBullet(GameObject b)
    {
        bullet = b;
    }

    public void fire(GameObject target, float speed, float damage, ElementType type, int specialEffectDuration, int specialEffectDmg, SpecialEffect se)
    {
        if (target == null)
            return;

        GameObject instBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
//        instBullet.transform.parent = GetComponentInParent<triggerEnemiesCollisionList>().gameObject.transform;
        instBullet.GetComponent<Bullet>().setBulletInfo(target, speed, damage, type,specialEffectDuration, specialEffectDmg, se);
    }
}
