using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChunkScene : MonoBehaviour
{
    public float chunkSizeX;
    public float chunkSizeZ;
    public ChunkMapLevel mapString;
    public NavMeshSurface surface;
    /// <summary>
    /// NOT UPDATED - USE GetChunkType(int x, int y)
    /// </summary>
    private ChunkType[,] map;
    public GameObject[] chunkPrefab;
    public Chunk[,] chunkMap;
    public bool[,] oldPath;
    public bool[,] path;
    public List<(int, int)> route;
    public SpawnManaging.SpawnManager spawner;
    public LineRenderer newPathRender;
    // Start is called before the first frame update
    void Start()
    {
        route = new List<(int, int)>();
        map = mapString.GetChunkType();
        chunkMap = new Chunk[mapString.sizeX, mapString.sizeY];
        path = new bool[mapString.sizeX, mapString.sizeY];
        oldPath = new bool[mapString.sizeX, mapString.sizeY];
        Build();
        if (!(chunkMap[0, 0].BFS())) Debug.LogError("ChunkScene.cs/Start() initial path failed");
        UpdateChunks();
        (int, int) spawn = GetFirstChunkOfType(ChunkType.spawnPoint);
        (int, int) target = GetFirstChunkOfType(ChunkType.endPoint);

        spawner.spawnPoints[0] = chunkMap[spawn.Item1, spawn.Item2].transform;
        spawner.target = chunkMap[target.Item1, target.Item2].transform;
    }
    private void OnValidate()
    {
        foreach(GameObject o in chunkPrefab)
        {
            if(o.GetComponent<Chunk>() == null)
            {
                Debug.LogError($"{o.name} is missing Chunk Component, but it's used as chunk in ChunkScene inside {gameObject.name}");
            }
        }
    }
    private void Build()
    {
        for (int y = 0; y < mapString.sizeY; y++)
        {
            for (int x = 0; x < mapString.sizeX; x++)
            {
                GameObject o = Instantiate(chunkPrefab[Random.Range(0, chunkPrefab.Length)],
                                            transform.position + Vector3.right * chunkSizeX * x
                                                               + Vector3.forward * chunkSizeZ * y,
                                            Quaternion.identity,
                                            transform);
                chunkMap[x, y] = o.GetComponent<Chunk>();
                chunkMap[x, y].x = x;
                chunkMap[x, y].y = y;
                chunkMap[x, y].type = map[x, y];
                chunkMap[x, y].owner = this;
            }
        }
    }
    public void UpdateChunks(bool forced = false)
    {
        int i = 0;
        if(!forced)
        {
            for (int y = 0; y < mapString.sizeY; y++)
            {
                for (int x = 0; x < mapString.sizeX; x++)
                {
                    chunkMap[x, y].UpdateChunk();
                    i++;
                }
            }
        }
        else
        {
            for (int y = 0; y < mapString.sizeY; y++)
            {
                for (int x = 0; x < mapString.sizeX; x++)
                {
                    if(oldPath[x,y] || path[x,y])
                    {
                       chunkMap[x, y].UpdateChunk();
                        i++;
                    }
                }
            }
        }
        oldPath = (bool[,])path.Clone();
    }

    public ChunkType GetChunkType(int x, int y) {
        return chunkMap[x, y].type;
    }

    public (int, int) GetFirstChunkOfType(ChunkType type) {

        (int, int) point = (0, 0);
        bool wasPointFound = false;
        for (int y = 0; y < mapString.sizeY; y++)
        {
            for (int x = 0; x < mapString.sizeX; x++)
            {
                if(chunkMap[x, y].type == type){
                    point = (x, y);
                    wasPointFound = true;
                    break;
                }
            }
            if (wasPointFound) break;
        }
        if (!wasPointFound){
            Debug.LogError($"ChunkScene.cs/GetFirstChunkOfType(): Point of type {type} wasn't found");
        }
        return point;
    }
    public void ShowNewPath()
    {
        CancelInvoke("HideNewPath");
        newPathRender.positionCount = this.route.Count;
        (int, int)[] route = this.route.ToArray();
        for(int i=0; i<this.route.Count; i++)
        {
            newPathRender.SetPosition(i, transform.position + Vector3.right * chunkSizeX * route[i].Item1
                                                               + Vector3.forward * chunkSizeZ * route[i].Item2 + Vector3.up * 0.5f);
        }
        newPathRender.enabled = true;
    }
    public void HideNewPath()
    {
        newPathRender.enabled = false;
    }
}
