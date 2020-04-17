using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestPooled : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("Disable", 10);
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
