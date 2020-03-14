using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    public void fire(GameObject target, float speed, float damage)
    {
        GameObject instBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        instBullet.GetComponent<Bullet>().setBulletInfo(target, speed, damage);
    }
}
