using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUnwalkableChunk : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        gameObject.SetActive(true);
    }

}
