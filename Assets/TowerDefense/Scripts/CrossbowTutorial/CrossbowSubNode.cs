using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Zinnia.Tracking.Collision.Active.Operation.Extraction;

public class CrossbowSubNode : MonoBehaviour
{
    [SerializeField]
    protected string TextToDisplay;
    [SerializeField]
    protected Text InfoForPlayer;
    [SerializeField]
    protected Canvas canvasToDisplayText;
    protected bool state;
    [SerializeField]
    protected CrossbowSubNode NextStep;
    [SerializeField]
    protected CrossbowSubNode PrevStep;
    public UnityEvent taskFinished;
    public UnityEvent taskFailed;

    virtual public void EnterStep()
    {
        Debug.Log("Wejście do " + gameObject.name);
        canvasToDisplayText.enabled = true;
        InfoForPlayer.text = TextToDisplay;
    }

    virtual public void ExitStep()
    {
        Debug.Log("Wyjscie z " + gameObject.name);
        canvasToDisplayText.enabled = false;
    }

    public void SetNextStep()
    {
        state = true;
        if (NextStep != null)
        {
            CrossbowMainNode.Instance.SetCurrentTutorial(NextStep);
            taskFinished.Invoke();
        }
    }

    public void SetPrevStep()
    {
        state = false;
        if (PrevStep != null)
        {
            taskFailed.Invoke();
            for (int i = CrossbowMainNode.Instance.ActiveTasks.Count - 1; i >= 0; i--)
            {
                if (CrossbowMainNode.Instance.ActiveTasks[i] != PrevStep)
                {
                    CrossbowMainNode.Instance.ActiveTasks[i].gameObject.SetActive(false);
                    CrossbowMainNode.Instance.ActiveTasks.RemoveAt(i);
                }
                else
                    break;
            }
            CrossbowMainNode.Instance.SetCurrentTutorial(PrevStep);
        }
    }

    public bool getState()
    {
        return state;
    }

}
