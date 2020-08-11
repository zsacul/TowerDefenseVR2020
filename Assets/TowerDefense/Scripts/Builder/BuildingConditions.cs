using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConditions : MonoBehaviour
{
    public bool obstacleAnywhere;
    public bool obstacleOnPath;
    public bool towerAnywhere;

    public static BuildingConditions Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AllowObstacleAnywhere(bool state)
    {
        obstacleAnywhere = state;
    }

    public void AllowObstacleOnPath(bool state)
    {
        obstacleOnPath = state;
    }

    public void AllowTowerAnywhere(bool state)
    {
        towerAnywhere = state;
    }

    public void AllowAll(bool state)
    {
        towerAnywhere = obstacleAnywhere = obstacleOnPath = state;
    }
}
