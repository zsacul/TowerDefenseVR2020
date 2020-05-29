using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuiltListener : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        this.GetComponent<InstructionsHandler>().TowerBuilt();
    }
}
