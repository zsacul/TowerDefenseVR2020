using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestNotPooled : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
