using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasHandler : GameEventListener
{
    private bool tutorialRuns;

    private void Start()
    {
        tutorialRuns = true;
    }
    public override void OnEventRaised(Object data)
    {
        if (tutorialRuns)
        {
            gameObject.SetActive(false);
            tutorialRuns = false;
        }
    }
}
