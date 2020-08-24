using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SpellsTutorialSubnode : MonoBehaviour
{
    [SerializeField]
    protected string TextToDisplay;
    [SerializeField]
    protected TextMeshProUGUI InfoForPlayer;
    [SerializeField]
    protected Canvas canvasToDisplayText;
    protected bool state;
    [SerializeField]
    protected SpellsTutorialSubnode NextStep;
    [SerializeField]
    protected SpellsTutorialSubnode PrevStep;
    public UnityEvent taskFinished;
    public UnityEvent taskFailed;

    virtual public void EnterStep()
    {
        gameObject.SetActive(true);
        Debug.Log("Wejście do " + gameObject.name);
        if (gameObject.activeSelf)
            Debug.Log(gameObject.name + " aktywny");
        //canvasToDisplayText.enabled = true;
        InfoForPlayer.text = TextToDisplay;
    }

    virtual public void ExitStep()
    {
        Debug.Log("Wyjscie z " + gameObject.name);
        //canvasToDisplayText.enabled = false;
    }

    public void SetNextStep()
    {
        state = true;
        if (NextStep != null)
        {
            SpellsTutorialMainNode.Instance.SetCurrentTutorial(NextStep);
            taskFinished.Invoke();
        }
    }

    public void SetPrevStep()
    {
        state = false;
        if (PrevStep != null)
        {
            taskFailed.Invoke();
            for (int i = SpellsTutorialMainNode.Instance.ActiveTasks.Count - 1; i >= 0; i--)
            {
                if (SpellsTutorialMainNode.Instance.ActiveTasks[i] != PrevStep)
                {
                    SpellsTutorialMainNode.Instance.ActiveTasks[i].gameObject.SetActive(false);
                    SpellsTutorialMainNode.Instance.ActiveTasks.RemoveAt(i);
                }
                else
                    break;
            }
            SpellsTutorialMainNode.Instance.SetCurrentTutorial(PrevStep);
        }
    }

    public bool getState()
    {
        return state;
    }
}
