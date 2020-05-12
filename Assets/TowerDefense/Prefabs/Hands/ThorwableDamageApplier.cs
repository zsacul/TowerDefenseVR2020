using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorwableDamageApplier : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.relativeVelocity.magnitude > 1.0)
            {
                float payload = collision.relativeVelocity.magnitude * damage;
                if (payload < 0.1f)
                    payload = 0.0f;

                collision.gameObject.BroadcastMessage("ApplyDamage", payload);
            }
 
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
