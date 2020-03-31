using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PrefabSet", menuName = "ScriptableObjects/PrefabSet", order = 3)]
public class PrefabsSet : ScriptableObject
{
    [Tooltip("Randomly choosen, can't build, can't path, used outside borderline")]
    public GameObject[] none;
    [Tooltip("index == 0 => straight, index == 1 => corner, without path index > 1 randomly choosen, can build, can path")]
    public GameObject[] empty;
    [Tooltip("index == 0 => inner corner, index == 1 => outer corner, straight index > 1 randomly choosen, can't build, can't path")]
    public GameObject[] border;
    [Tooltip("Randomly choosen, can't build, can't path")]
    public GameObject[] naturalObstacle;
    [Tooltip("Randomly choosen, can build, can't path")]
    public GameObject[] playerObstacle;
    [Tooltip("choosen by player, can build, can't path")]
    public GameObject[] tower;
    [Tooltip("single instance, randomly choosen, can't build, can path, enemy spawn point")]
    public GameObject[] spawnPoint;
    [Tooltip("single instance, randomly choosen, can't build, can path, enemy target")]
    public GameObject[] endPoint;
}
