using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTowerQuest : BuildTutorialQuest
{
    public override void SetNextQuest()
    {
        state = true;
        QuestFinished.Invoke();
        BuildTutorialManager.Instance.EndTutorial();
    }
}