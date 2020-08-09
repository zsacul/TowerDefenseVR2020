using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTowerQuest : BuildTutorialQuest
{
    //public GameObject ButtonStartWave;

    public override void EnterQuest()
    {
        base.EnterQuest();
        BuildTutorialManager.Instance.currentState = BuildingTutorialState.upgradeTower;
    }

    public override void SetNextQuest()
    {
        state = true;
        QuestFinished.Invoke();
        BuildTutorialManager.Instance.EndTutorial();
        //ButtonStartWave.SetActive(true);
    }
}