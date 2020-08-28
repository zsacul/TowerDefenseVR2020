using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TeleportSubNode : MonoBehaviour
{
    [SerializeField]
    protected string TextToDisplay;
    [SerializeField]
    protected TextMeshProUGUI InfoForPlayer;
    [SerializeField]
    protected Canvas canvasToDisplayText;
    protected bool state;
    protected bool doneInPast;
    [SerializeField]
    protected TeleportSubNode NextStep;
    [SerializeField]
    protected TeleportSubNode PrevStep;
    public UnityEvent taskFinished;
    public UnityEvent taskFailed;
    public UnityEvent taskFinishedFirstTime;

    virtual public void EnterStep()
    {
        //Debug.Log("Wejście do " + gameObject.name);
        canvasToDisplayText.enabled = true;
        InfoForPlayer.text = TextToDisplay;
    }

    virtual public void ExitStep()
    {
        //Debug.Log("Wyjscie z " + gameObject.name);
        canvasToDisplayText.enabled = false;
    }

    virtual public void SetNextStep()
    {
        state = true;
        if (NextStep != null)
        {
            TeleportMainNode.Instance.SetCurrentTutorial(NextStep);
            taskFinished.Invoke();
            if (!doneInPast)
            {
                doneInPast = true;
                taskFinishedFirstTime.Invoke();
            }
        }
    }

    virtual public void SetPrevStep()
    {
        state = false;
        if (PrevStep != null)
        {
            taskFailed.Invoke();
            for( int i = TeleportMainNode.Instance.ActiveTasks.Count - 1; i >= 0; i--)
            {
                if (TeleportMainNode.Instance.ActiveTasks[i] != PrevStep)
                {
                    TeleportMainNode.Instance.ActiveTasks[i].gameObject.SetActive(false);
                    TeleportMainNode.Instance.ActiveTasks.RemoveAt(i);
                }
                else
                    break;
            }
            TeleportMainNode.Instance.SetCurrentTutorial(PrevStep);
        }
    }
    
}
