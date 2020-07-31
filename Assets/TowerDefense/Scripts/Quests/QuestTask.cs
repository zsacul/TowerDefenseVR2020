using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestTask : MonoBehaviour, IQuest
{
    public UnityEvent taskFinished;
    private int current;
    [SerializeField] private int target;

    public void IncrementTask()
    {
        current += 1;
        if(current == target)
        {
            taskFinished.Invoke();
        }
    }

    public (int,int) GetTaskStateAsPair()
    {
        return (current, target);
    }

    public float GetTaskStateProc()
    {
        return (float)current / (float)target;
    }

    public void AddListenerFinish(UnityAction action)
    {
        taskFinished.AddListener(action);
    }

    public bool GetState()
    {
        return current >= target;
    }
}
