using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerQuest : BuildTutorialQuest
{
    public override void EnterQuest()
    {
        base.EnterQuest();
        BuildTutorialManager.Instance.currentState = BuildingTutorialState.buildTower;
    }
}
