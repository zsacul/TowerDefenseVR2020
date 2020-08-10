using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObstacleQuest : BuildTutorialQuest
{
    public override void EnterQuest()
    {
        NodeMenu.SetPersistantState("build", true);
        NodeMenu.SetPersistantState("obstacle", true);
        base.EnterQuest();
        BuildTutorialManager.Instance.currentState = BuildingTutorialState.selectObstacle;
    }
}
