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
    protected string infoForPlayer;
    [SerializeField]
    protected TextMeshProUGUI textHolder;
    protected bool doneInPast;

    virtual public void EnterQuest()
    {
        Debug.Log("textHolder.text = infoForPlayer inside " + gameObject.name);

        textHolder.text = infoForPlayer;
        //Debug.Log("Wejście do " + gameObject.name);
    }

    virtual public void ExitQuest()
    {
        Debug.Log("textHolder.text = \"\" inside " + gameObject.name);
        //textHolder.text = "";
        //Debug.Log("Wyjscie z " + gameObject.name);
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

    public override void OnEventRaised(Object data)
    {
        SetNextQuest();
    }
}
