using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TargetHit : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    public UnityEvent Hited;
    public int NumOfHits;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.transform.parent = this.gameObject.transform;
            Hited.Invoke();
        }
    }
}