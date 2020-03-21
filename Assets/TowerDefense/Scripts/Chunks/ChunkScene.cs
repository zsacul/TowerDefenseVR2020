using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScene : MonoBehaviour
{
    public float chunkSizeX;
    public float chunkSizeZ;
    public ChunkMapLevel mapString;
    private ChunkType[,] map;
    public GameObject[] chunkPrefab;
    public Chunk[,] chunkMap;
    public bool[,] path;
    // Start is called before the first frame update
    void Start()
    {
        map = mapString.GetChunkType();
        chunkMap = new Chunk[mapString.sizeX, mapString.sizeY];
        path = new bool[mapString.sizeX, mapString.sizeY];
        Build();
        UpdateChunks();
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
    public void Build()
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
    public void UpdateChunks()
    {
        for (int y = 0; y < mapString.sizeY; y++)
        {
            for (int x = 0; x < mapString.sizeX; x++)
            {
                chunkMap[x, y].UpdateChunk();
            }
        }
    }
}
