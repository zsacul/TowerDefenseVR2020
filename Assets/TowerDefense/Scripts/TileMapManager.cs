using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileMapManager : MonoBehaviour
{
    public int TilemapSizeX, TilemapSizeY;
    public float TilemapChunkSize;
    public GameObject TilemapChunk;
    public GameObject TilemapBlocker;
    public NavMeshSurface surface;
    private GameObject[,] TileChunksArray;

    // Start is called before the first frame update
    void Start() {
        
        TileChunksArray = new GameObject[TilemapSizeX,TilemapSizeY];
        // Create the game map.
        for (int i = 0; i < TilemapSizeX; i++)
            for (int j = 0; j < TilemapSizeY; j++)
            {
                GameObject Tile = Instantiate(TilemapChunk, new Vector3((float)i * TilemapChunkSize, 0.1f, (float)j * TilemapChunkSize), Quaternion.identity);
                Tile.transform.SetParent(transform);
                TileChunksArray[i,j] = Tile;

                GameObject Block = Instantiate(TilemapBlocker, new Vector3((float)i * TilemapChunkSize, 0.5f, (float)j * TilemapChunkSize), Quaternion.identity);
                Block.transform.SetParent(Tile.transform);
            }

        surface.BuildNavMesh(); // Bake navmesh. Call this everytime there is an update of the map.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
