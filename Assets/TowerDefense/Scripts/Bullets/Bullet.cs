using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    float speed;
    float damage;
    string type;

    public void setBulletInfo(GameObject t, float s, float d, string typ)
    {
        target = t;
        speed = s;
        damage = d;
        type = typ;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        transform.LookAt(target.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.BroadcastMessage("ApplyDamage", damage);
        }
        else if( collision.gameObject.tag != "Bullet")
            destroyBullet();
    }

    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
