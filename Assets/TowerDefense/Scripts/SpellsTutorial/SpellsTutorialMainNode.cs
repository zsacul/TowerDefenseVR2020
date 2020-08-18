using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellsTutorialMainNode : MonoBehaviour, IQuest
{
    public UnityEvent TasksFinished;
    public List<SpellsTutorialSubnode> ActiveTasks = new List<SpellsTutorialSubnode>();
    [SerializeField]
    BuildTutorialManager BuildTutorialManager;
    [SerializeField]
    TeleportMainNode TeleportTutorial;

    bool state;
    SpellsTutorialSubnode currentTutorial;
    [SerializeField]
    SpellsTutorialSubnode firstStep;
    public bool isElementChose;
    public ElementType elementChose;

    private static SpellsTutorialMainNode instance;
    public static SpellsTutorialMainNode Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SpellsTutorialMainNode>();

            if (instance == null)
                Debug.LogError("Nie ma tutoriala od teleportu na scenie");

            return instance;
        }
    }

    public void SetCurrentTutorial(SpellsTutorialSubnode t)
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

    public SpellsTutorialSubnode GetCurrentTutorial()
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
