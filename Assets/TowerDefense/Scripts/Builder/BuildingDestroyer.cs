using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestroyer : MonoBehaviour
{
    private Chunk chunk;

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
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowNewPath();
        }
    }

    public void DestroyBuilding()
    {
        int moneyToReturn;
        switch (chunk.type)
        {
            case ChunkType.playerObstacle:
                moneyToReturn = BuildManager.Instance.playerObstacleCost / 2;
                break;
            case ChunkType.tower:
                moneyToReturn = BuildManager.Instance.towerCost / 2;
                break;
            default:
                Debug.LogError("Trying to destroy building that can't be destroyed");
                return;
        }
        chunk.ChangeType(ChunkType.empty);
    }

    public void ShowNewPath()
    {
        chunk.BFS(false, true);
    }
}
