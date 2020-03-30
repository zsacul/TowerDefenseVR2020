using System.Collections;
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
                
                return PathExist();
                break;
            case ChunkType.playerObstacle:
                
                return PathExist();
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

    public bool PathExist() {

        bool endPointReached = false;
        HashSet<(int, int)> wasVisited = new HashSet<(int, int)>();
        Queue<(int, int)> Q = new Queue<(int, int)>();
        (int, int) spawnPoint = owner.GetFirstChunkOfType(ChunkType.spawnPoint);
        (int, int) endPoint = owner.GetFirstChunkOfType(ChunkType.endPoint);
        Q.Enqueue(spawnPoint);
        wasVisited.Add(spawnPoint);


        while(Q.Count != 0 || !endPointReached) 
        {
            (int, int) element = Q.Dequeue();
            if (element == endPoint)
            {
                endPointReached = true;
                break;
            }
            
            wasVisited.Add(element);
            int xCord = element.Item1;
            int yCord = element.Item2;

            // (x, y) -> (x+1,y), (x-1,y), (x, y+1), (x, y-1)
            for(int y = yCord + 1; y <= yCord - 1; y -= 2) 
            {
                ChunkType chunkType = owner.GetChunkType(xCord, y);
                if(chunkType == ChunkType.empty && (xCord != this.x && y != this.y) && !wasVisited.Contains((xCord, y))) {
                    Q.Enqueue((xCord, y));
                }
            }
            for(int x = xCord + 1; x <= xCord - 1; y -= 2) 
            {
                ChunkType chunkType = owner.GetChunkType(xCord, y);
                if(chunkType == ChunkType.empty && (x != this.x && yCord != this.y) && !wasVisited.Contains((xCord, y))) {
                    Q.Enqueue((xCord, y));
                }
            }
        }

        return endPointReached;
    }
}
