using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOverListener : GameEventListener
{
    [SerializeField]
    private GameEvent waveOver;

    public override void OnEventRaised(Object data)
    {
        Debug.Log("KONIEC FALI");
        gameObject.GetComponent<BuildManager>().ChangeWaveStatus(true);
    }
}
