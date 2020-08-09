using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TargetHit : MonoBehaviour
{
    public UnityEvent Hited;
    [NonSerialized]
    public int NumOfHits;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Kolizja z " + collision.gameObject.name);
        if(collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.transform.parent = this.gameObject.transform;
            Hited.Invoke();
        }
    }
}