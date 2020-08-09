using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

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
    public void TeleportState(bool state)
    {
        RightRaycast.Instance.TurnTeleport(state);
    }
}
