using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject SpearPrefab;
    [SerializeField]
    float RespawnTime;

    private GameObject spear;
    private bool isRespawning = false;
    private float timeCounter = 0;

    private void Start()
    {
        spear = Instantiate(SpearPrefab);
        spear.transform.position = this.transform.position;
        spear.transform.parent = this.transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Destroy(spear);
            StartRespawningSpear();
        }
        if(isRespawning)
        {
            CheckSpawnSpear();
        }
    }

    private void CheckSpawnSpear()
    {
        if(timeCounter > RespawnTime)
        {
            spear = Instantiate(SpearPrefab);
            spear.transform.position = this.transform.position;
            spear.transform.parent = this.transform;
            timeCounter = 0;
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }

    //calling starts timer to spawning spear
    public void StartRespawningSpear()
    {
        isRespawning = true;
    }
}
