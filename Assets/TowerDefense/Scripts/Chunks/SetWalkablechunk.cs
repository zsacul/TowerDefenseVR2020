using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWalkablechunk : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        gameObject.SetActive(false);
    }
}
