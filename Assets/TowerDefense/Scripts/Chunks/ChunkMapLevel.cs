using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameLevelMap", menuName = "ScriptableObjects/Map", order = 3)]
public class ChunkMapLevel : ScriptableObject
{
    public int sizeX;
    public int sizeY;
    [TextArea]
    [Tooltip("space between each tileType symbol, new line before new line")]
    [Header("7 = endPoint")]
    [Header("6 = spawnPoint")]
    [Header("5 = tower")]
    [Header("4 = playerObstacle")]
    [Header("3 = naturalObstacle")]
    [Header("2 = border")]
    [Header("1 = empty")]
    [Header("0 = none")]
    public string map;
    
    public ChunkType[,] GetChunkType()
    {
        string[] words = map.Split(' ', '\n');
        ChunkType[,] type = new ChunkType[sizeX, sizeY];
        for(int y = 0; y < sizeY; y++)
        {
            for(int x = 0; x < sizeX; x++)
            {
                type[x, y] = (ChunkType)int.Parse(words[sizeX * y + x]);
                Debug.Log(type[x, y]);
            }
        }
        return type;
    }
}
