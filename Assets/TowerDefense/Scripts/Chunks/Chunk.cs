﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chunk : MonoBehaviour
{
    public ChunkType type;
    public PrefabsSet prefabs;
    private bool canBeModified;
    [Tooltip("if true => can change type in runtime with ChangeType function")]
    public UnityEvent changeTypeEvent;
    [HideInInspector]
    public int x, y;
    [HideInInspector]
    public ChunkScene owner;
    public int choice;
    private GameObject currentObject;
    public bool ChangeType(ChunkType newType, int choice = 0)
    {
        if(canBeModified && ValidOperation(newType))
        {
            this.choice = choice;
            if (type != newType)
            {
                type = newType;
                changeTypeEvent.Invoke();
                UpdateChunk();
                return true;
            }
        }
        return false;
    }
    bool ValidOperation(ChunkType newType)
    {
        switch(newType)
        {
            case ChunkType.tower:
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    for (int x = this.x - 1; x <= this.x + 1; x++)
                    {
                        if (owner.chunkMap[x, y].type == ChunkType.tower)
                        {
                            return false;
                        }
                    }
                }
                //check Path TODO
                return true;
                break;
            case ChunkType.playerObstacle:
                //check Path TODO
                return true;
                break;
            default:
                return false;
        }
    }
    public void UpdateChunk()
    {
        if(currentObject != null)
        {
            Destroy(currentObject);
        }
        switch(type)
        {
            case ChunkType.none:
                currentObject = Instantiate(prefabs.none[Random.Range(0, prefabs.none.Length)], transform);
                break;
            case ChunkType.empty:
                if (owner.path[x, y])
                {
                    //TODO generate path from proper tiles with proper rotations
                }
                else
                {
                    currentObject = Instantiate(prefabs.empty[Random.Range(0, prefabs.empty.Length)], transform);
                }
                break;
            case ChunkType.border:
                //TODO (choose correct tile and rotation based on neighbours)
                currentObject = Instantiate(prefabs.border[Random.Range(0, prefabs.border.Length)], transform);
                break;
            case ChunkType.naturalObstacle:
                currentObject = Instantiate(prefabs.naturalObstacle[Random.Range(0, prefabs.naturalObstacle.Length)], transform);
                break;
            case ChunkType.playerObstacle:
                currentObject = Instantiate(prefabs.playerObstacle[Random.Range(0, prefabs.playerObstacle.Length)], transform);
                break;
            case ChunkType.tower:
                currentObject = Instantiate(prefabs.tower[choice], transform);
                break;
            case ChunkType.spawnPoint:
                //TODO (choose correct rotation based on neighbours
                currentObject = Instantiate(prefabs.spawnPoint[Random.Range(0, prefabs.spawnPoint.Length)], transform);
                break;
            case ChunkType.endPoint:
                //TODO (choose correct rotation based on neighbours
                currentObject = Instantiate(prefabs.endPoint[Random.Range(0, prefabs.endPoint.Length)], transform);
                break;
        }
    }
}