using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildTutorialQuest : GameEventListener
{
    public UnityEvent QuestFinished;
    public UnityEvent QuestFinishedFirstTime;

    protected bool state;
    [SerializeField]
    protected BuildTutorialQuest nextQuest;
    [SerializeField]
    protected BuildTutorialQuest prevQuest;
    [SerializeField]
    protected string infoForPlayer;
    [SerializeField]
    protected TextMeshProUGUI textHolder;
    protected bool doneInPast;

    virtual public void EnterQuest()
    {
        textHolder.text = infoForPlayer;
    }

    virtual public void ExitQuest()
    {
    }

    public virtual void SetNextQuest()
    {
        state = true;
        if(nextQuest != null && BuildTutorialManager.Instance.CurrentQuest() != nextQuest && this.enabled)
        {
            //Debug.Log(gameObject.name + " calls SetNextQuest()");
            BuildTutorialManager.Instance.SetCurrentQuest(nextQuest);
            QuestFinished.Invoke();
            if (!doneInPast)
            {
                QuestFinishedFirstTime.Invoke();
                doneInPast = true;
            }
            ExitQuest();
        }
    }

    public virtual void SetPrevQuest()
    {
        if(prevQuest != null && BuildTutorialManager.Instance.CurrentQuest() != nextQuest && this.enabled)
        {
            BuildTutorialManager.Instance.SetCurrentQuest(prevQuest);
        }
    }

    public override void OnEventRaised(Object data)
    {
        SetNextQuest();
    }
}
