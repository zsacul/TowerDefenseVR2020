using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOverListener : GameEventListener
{
    [SerializeField]
    private GameEvent waveOver;

    public override void OnEventRaised(Object data)
    {
        gameObject.GetComponent<BuildManager>().ChangeWaveStatus(true);
        AudioManager.Instance.PlayAmbientBGM();
        NodeMenu.Switch(true);
    }
}
