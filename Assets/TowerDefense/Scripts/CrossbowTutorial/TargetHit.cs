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

    bool hited;
    
    // Start is called before the first frame update
    void Start()
    {
        hited = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            hited = true;
            collision.gameObject.transform.parent = this.gameObject.transform;
            Hited.Invoke();
        }
    }
}