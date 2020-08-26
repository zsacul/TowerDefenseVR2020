using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrossbowMainNode : MonoBehaviour, IQuest
{
    public UnityEvent TasksFinished;
    public List<CrossbowSubNode> ActiveTasks = new List<CrossbowSubNode>();
    [SerializeField]
    BuildTutorialManager BuildTutorialManager;
    [SerializeField]
    TeleportMainNode TeleportTutorial;

    bool state;
    CrossbowSubNode currentTutorial;
    [SerializeField]
    CrossbowSubNode firstStep;

    private static CrossbowMainNode instance;
    public static CrossbowMainNode Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<CrossbowMainNode>();

            if (instance == null)
                Debug.LogError("Nie ma tutoriala od teleportu na scenie");

            return instance;
        }
    }

    public void SetCurrentTutorial(CrossbowSubNode t)
    {
        if (t != null && (currentTutorial == null || t != currentTutorial) && !state)
        {
            if (currentTutorial != null)
                currentTutorial.ExitStep();
            ActiveTasks.Add(t);
            currentTutorial = t;
            currentTutorial.gameObject.SetActive(true);
            currentTutorial.EnterStep();
        }
    }

    public void SetTutorialDone()
    {
        state = true;
        ClearTasks();
        TasksFinished.Invoke();
        if (BuildTutorialManager != null)
        {
            BuildTutorialManager.gameObject.SetActive(true);
            BuildTutorialManager.StartTutorial();
        }
    }

    public CrossbowSubNode GetCurrentTutorial()
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
        Destroy(TeleportTutorial.gameObject);
        SetCurrentTutorial(firstStep);
    }

    public void Skip()
    {
        foreach (Transform child in transform)
        {
            var questScript = child.GetComponent<CrossbowSubNode>();
            if (questScript)
            {
                questScript.SetNextStep();
            }
        }
    }

    public void ClearTasks()
    {
        Debug.Log("CLEAR TASKS CROSSBOW TUTORIAL");
        for (int i = ActiveTasks.Count - 2; i >= 0; i--)
        {
            Debug.Log("wylaczono obiekt " + ActiveTasks[i].gameObject.name);
            ActiveTasks[i].gameObject.SetActive(false);
        }
        ActiveTasks.Clear();

        Debug.Log("Wszystko wyczyszczone");
    }
}
