using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeight : MonoBehaviour
{
    public float towerHeight = 1.0f;
    void Start()
    {
        towerHeight = UnityEngine.Random.Range(0.7f, 1.0f);
        transform.localScale = new Vector3(transform.localScale.x, towerHeight, transform.localScale.z);
    }

  
}
