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
        hoverOn = false;
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
        int buildingCost;

        switch (chunk.type)
        {
            case ChunkType.playerObstacle:
                GetComponent<Animator>().SetBool("Destroyed", true);
                buildingCost = BuildManager.Instance.playerObstacleCost;
                break;
            case ChunkType.tower:
                Debug.Log("Typ wieży: " + chunk.GetTowerType());
                buildingCost = BuildManager.Instance.towerCost;
                switch (chunk.GetTowerType())
                {
                    case TowerType.basic:
                        break;
                    case TowerType.ice:
                        buildingCost += BuildManager.Instance.iceTowerCost;
                        break;
                    case TowerType.fire:
                        buildingCost += BuildManager.Instance.fireTowerCost;
                        break;
                    case TowerType.lightning:
                        buildingCost += BuildManager.Instance.electricTowerCost;
                        break;
                    default:
                        Debug.LogError("Unknown tower is being destroyed");
                        return;
                }
                GetComponent<Animator>().SetTrigger("hide");
                break;
            default:
                Debug.LogError("Trying to destroy building that can't be destroyed");
                return;
        }
        moneyToReturn = (int)Math.Ceiling(buildingCost / returnedMoneyRate);
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
            hoverOn = false;
        }
    }

    public void ShowNewPath()
    {
        chunk.BFS(false, true);
    }

}
