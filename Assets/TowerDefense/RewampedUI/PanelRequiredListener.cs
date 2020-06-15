using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRequiredListener : GameEventListener
{

    public override void OnEventRaised(Object data)
    {
        transform.GetComponent<GenericNBoxSelector>().Respawn();
    }
}

