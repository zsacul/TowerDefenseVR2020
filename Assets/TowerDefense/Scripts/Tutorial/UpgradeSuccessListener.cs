using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSuccessListener : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        this.GetComponent<InstructionsHandler>().TowerUpgraded();
    }
}
