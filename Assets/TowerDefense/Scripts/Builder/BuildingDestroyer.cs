using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingDestroyer : MonoBehaviour
{
    private Chunk chunk;
    public float returnedMoneyRate;
    private bool hoverOn;

    private void Start()
    {
        chunk = transform.parent.GetComponent<Chunk>();
        if (!chunk)
        {
            Debug.LogError("Chunk script missing");
        }
    }

    public void Update()
    {
        
    }

    public void DestroyBuilding()
    {
        int moneyToReturn;
        switch (chunk.type)
        {
            case ChunkType.playerObstacle:
                moneyToReturn = (int)Math.Ceiling(BuildManager.Instance.playerObstacleCost / returnedMoneyRate);
                break;
            case ChunkType.tower:
                moneyToReturn = (int)Math.Ceiling(BuildManager.Instance.towerCost / returnedMoneyRate);
                break;
            default:
                Debug.LogError("Trying to destroy building that can't be destroyed");
                return;
        }
        if (chunk.ChangeType(ChunkType.empty))
        {
            BuildManager.Instance.AddMoney(moneyToReturn);
            BuildManager.BurstMoneyEffect(moneyToReturn, transform.position);
        }
        else
        {
            Debug.LogError("Building could not be destroyed");
        }
    }

    public void HoverOn()
    {
        if(!hoverOn)
        {
            hoverOn = true;
            ShowNewPath();
        }
    }

    public void HoverOff()
    {
        if(hoverOn)
        {
            chunk.owner.HideNewPath();
        }
    }

    public void ShowNewPath()
    {
        chunk.BFS(false, true);
    }

}
