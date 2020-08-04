using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTowerQuest : BuildTutorialQuest
{
    public GameObject button; 
    public override void SetNextQuest()
    {
        state = true;
        QuestFinished.Invoke();
        BuildTutorialManager.Instance.EndTutorial();
        button.SetActive(true);
    }
}