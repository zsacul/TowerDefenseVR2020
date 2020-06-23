using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour, IQuest
{
    private IQuest[] tasks;
    public UnityEvent finish;
    private bool state;
    
    private void Start()
    {
        tasks = GetComponentsInChildren<IQuest>().Skip(1).ToArray();

        foreach(IQuest task in tasks)
        {
            task.AddListenerFinish(() => {
                UpdateQuest();
            });
        }
    }
    private void UpdateQuest()
    {
        try
        {
            tasks.First(x => !x.GetState());
        }
        catch (System.Exception)
        {
            finish.Invoke();
            state = true;
        }
    }
    public void AddListenerFinish(UnityAction action)
    {
        finish.AddListener(action);
    }

    public bool GetState()
    {
        return state;
    }
}