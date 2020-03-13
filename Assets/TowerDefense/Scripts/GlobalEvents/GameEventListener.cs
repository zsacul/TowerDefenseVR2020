using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener : MonoBehaviour
{
    public GameEvent listenedEvent;

    abstract public void OnEventRaised(Object data);
    private void OnEnable()
    {
        listenedEvent.RegisterListener(this);
    }
    private void OnDestroy()
    {
        listenedEvent.UnregisterListener(this);
    }
    
}
