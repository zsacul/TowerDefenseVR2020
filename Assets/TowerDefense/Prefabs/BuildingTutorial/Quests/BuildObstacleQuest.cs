using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObstacleQuest : BuildTutorialQuest
{
    public override void EnterQuest()
    {
        base.EnterQuest();
        BuildTutorialManager.Instance.currentState = BuildingTutorialState.buildObstacle;
    }

    private void Update()
    {
        if(BuildManager.Instance.selectedBuilding != ChunkType.playerObstacle)
        {
            SetPrevQuest();
        }
    }
}
