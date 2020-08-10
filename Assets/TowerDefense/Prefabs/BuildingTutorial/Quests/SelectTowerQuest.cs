using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTowerQuest : BuildTutorialQuest
{
    public override void EnterQuest()
    {
        NodeMenu.SetPersistantState("tower", true);
        base.EnterQuest();
        BuildTutorialManager.Instance.currentState = BuildingTutorialState.selectTower;
    }
}
