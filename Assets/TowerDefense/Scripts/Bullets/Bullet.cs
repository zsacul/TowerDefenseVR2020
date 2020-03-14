using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    float speed;
    float damage;

    public void setBulletInfo(GameObject t, float s, float d)
    {
        target = t;
        speed = s;
        damage = d;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.BroadcastMessage("ApplyDamage", damage);
        }
        destroyBullet();
    }

    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
