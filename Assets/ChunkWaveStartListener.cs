using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWaveStartListener : GameEventListener
{
    [SerializeField]
    private GameEvent waveOver;

    public override void OnEventRaised(Object data)
    {   
        gameObject.GetComponent<IsWalkable>().ChangeIsWalkable(true);
    }
}
