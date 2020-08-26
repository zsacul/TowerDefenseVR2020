using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class TeleportMainNode : MonoBehaviour, IQuest
{
    public UnityEvent TasksFinished;
    public List<TeleportSubNode> ActiveTasks = new List<TeleportSubNode>();
    [SerializeField]
    CrossbowMainNode CrossbowMainNode;
    bool state;
    [NonSerialized]
    public bool teleDone;
    TeleportSubNode currentTutorial;
    [SerializeField]
    TeleportSubNode firstStep;
    private static TeleportMainNode instance;
    public static TeleportMainNode Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TeleportMainNode>();

            if (instance == null)
                Debug.LogError("Nie ma tutoriala od teleportu na scenie");

            return instance;
        }
    }

    public void SetCurrentTutorial(TeleportSubNode t)
    {
        if (t != null && (currentTutorial == null || t != currentTutorial) && !state) {
            if(currentTutorial != null)
                currentTutorial.ExitStep();
            ActiveTasks.Add(t);
            currentTutorial = t;
            currentTutorial.gameObject.SetActive(true);
            currentTutorial.EnterStep();
        }
    }

    public void SetTutorialDone()
    {
        CrossbowMainNode.gameObject.SetActive(true);
        state = true;
        ClearTasks();
        TasksFinished.Invoke();
    }

    public TeleportSubNode GetCurrentTutorial()
    {
        return currentTutorial;
    }

    public void AddListenerFinish(UnityAction action)
    {
        TasksFinished.AddListener(action);
    }

    public bool GetState()
    {
        return state;
    }

    public void StartThisTutorial()
    {
        SetCurrentTutorial(firstStep);
    }

    public void ClearTasks()
    {
        for(int i = ActiveTasks.Count - 2; i >= 0; i--)
        {
            Debug.Log("wylaczono obiekt " + ActiveTasks[i].gameObject.name);
            ActiveTasks[i].gameObject.SetActive(false);
        }
        ActiveTasks.Clear();

        Debug.Log("Wszystko wyczyszczone");
    }

    public void Skip()
    {
        foreach(Transform child in transform)
        {
            var questScript = child.GetComponent<TeleportSubNode>();
            if (questScript)
            {
                questScript.SetNextStep();
            }
        }
    }

    private void Start()
    {
        teleDone = false;
    }
}
