using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    float speed;
    float damage;
    TowerType type;
    bool readyToDestroy;

    public Bullet(GameObject t, float s, float d, TowerType typ)
    {
        setBulletInfo(t, s, d, typ);
    }

    public void setBulletInfo(GameObject t, float s, float d, TowerType typ)
    {
        target = t;
        speed = s;
        damage = d;
        type = typ;
        readyToDestroy = false;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        transform.LookAt(target.transform);
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

    public TowerType GetBulletType()
    {
        return type;
    }
}
