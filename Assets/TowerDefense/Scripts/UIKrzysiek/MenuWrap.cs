using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MenuWrap", menuName = "ScriptableObjects/Wrap", order = 5)]
public class MenuWrap : ScriptableObject
{
    public void BuildTower()
    {
        BuildManager.BuildTower();
    }
    public void BuildObstacle()
    {
        BuildManager.BuildObstacle();
    }
    public void ClearBuildMode()
    {
        BuildManager.ClearBuildMode();
    }
}
