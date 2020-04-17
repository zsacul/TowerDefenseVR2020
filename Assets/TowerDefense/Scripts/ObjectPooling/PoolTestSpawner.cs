using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestSpawner : MonoBehaviour
{
    public GameObject prefabPooled;
    public GameObject prefabNoPooling;
    public float spawnRate;
    public bool usePooling;
    private void Start()
    {
        InvokeRepeating("Spawn", 0, spawnRate);
    }
    void Spawn()
    {
        if(usePooling)
        {
            PoolManager.PoolInstantiate(prefabPooled, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(prefabNoPooling, transform.position, transform.rotation, null);
        }
    }
}
