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
        obstacleAnywhere = false;
        obstacleOnPath = false;
        towerAnywhere = false;
    }

    public void allowObstacleAnywhere(bool state)
    {
        obstacleAnywhere = state;
    }

    public void allowObstacleOnPath(bool state)
    {
        obstacleOnPath = state;
    }

    public void allowTowerAnywhere(bool state)
    {
        towerAnywhere = state;
    }
}
