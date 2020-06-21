using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfWaveListener : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        this.GetComponent<InstructionsHandler>().EndOfWave();
    }
}
