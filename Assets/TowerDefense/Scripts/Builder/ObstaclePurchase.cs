﻿using UnityEngine;
using System.Collections;

public class ObstaclePurchase : MonoBehaviour
{
    BuildManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            if (buildManager.ActiveBuildingInfo.Item1 == ChunkType.playerObstacle)
            {
                buildManager.ChooseNone();
            }
            else
            {
                buildManager.ChooseObstacle();
            }
        }
    }
}
