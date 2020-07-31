using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildTutorialQuest : GameEventListener
{
    public UnityEvent QuestFinished;
    public GameObject InstructionsPanel;

    protected bool state;
    [SerializeField]
    protected BuildTutorialQuest nextQuest;

    virtual public void EnterQuest()
    {
        InstructionsPanel.SetActive(true);
        Debug.Log("Wejście do " + gameObject.name);
    }

    virtual public void ExitQuest()
    {
        InstructionsPanel.SetActive(false);
        Debug.Log("Wyjscie z " + gameObject.name);
    }

    public virtual void SetNextQuest()
    {
        state = true;
        if(nextQuest != null && BuildTutorialManager.Instance.CurrentQuest() != nextQuest && this.enabled)
        {
            Debug.Log(gameObject.name + " calls SetNextQuest()");
            BuildTutorialManager.Instance.SetCurrentQuest(nextQuest);
            QuestFinished.Invoke();
            ExitQuest();
        }
    }

    public override void OnEventRaised(Object data)
    {
        SetNextQuest();
    }
}
