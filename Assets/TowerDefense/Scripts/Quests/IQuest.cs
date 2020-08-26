using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IQuest
{
    void AddListenerFinish(UnityAction action);
    bool GetState();

}
