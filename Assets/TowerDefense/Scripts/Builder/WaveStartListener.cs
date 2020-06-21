using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartListener : GameEventListener
{
    [SerializeField]
    private GameEvent waveStart;

    public override void OnEventRaised(Object data)
    {
        gameObject.GetComponent<BuildManager>().ChangeWaveStatus(false);
        AudioManager.Instance.PlayActionBGM();
    }
}
