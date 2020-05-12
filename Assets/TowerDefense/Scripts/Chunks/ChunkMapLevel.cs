using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "GameLevelMap", menuName = "ScriptableObjects/Map", order = 3)]
public class ChunkMapLevel : ScriptableObject
{
    public int sizeX;
    public int sizeY;
    [TextArea(60, 60)]
    [Tooltip("space between each tileType symbol, new line before new line")]
    [Header("right mouse button on 'map' to show functions")]
    [Header("7 = endPoint")]
    [Header("6 = spawnPoint")]
    [Header("5 = tower")]
    [Header("4 = playerObstacle")]
    [Header("3 = naturalObstacle")]
    [Header("2 = border")]
    [Header("1 = empty")]
    [Header("0 = none")]
    [ContextMenuItem("Generate Empty", "GenerateEmpty")]
    [ContextMenuItem("ctrl + z", "Ctrl_Z")]
    public string map;
    private string lastMap;
    public ChunkType[,] GetChunkType()
    {
        char[] mapCleared = new string((from c in map
                          where !char.IsWhiteSpace(c)
                          select c
               ).ToArray()).ToArray();
        ChunkType[,] type = new ChunkType[sizeX, sizeY];
        for(int y = 0; y < sizeY; y++)
        {
            for(int x = 0; x < sizeX; x++)
            {
                type[x, y] = (ChunkType)int.Parse(mapCleared[sizeX * y + x].ToString());
            }
        }
        return type;
    }
    void GenerateEmpty()
    {
        lastMap = map;
        map = string.Empty;
        ChunkType[,] type = new ChunkType[sizeX, sizeY];
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                map += 0;
            }
            map += "\n";
        }
    }
    void Ctrl_Z()
    {
        string temp = map;
        map = lastMap;
        lastMap = temp;
    }
}
