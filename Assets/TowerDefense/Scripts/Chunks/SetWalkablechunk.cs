using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWalkablechunk : GameEventListener
{
    [SerializeField]
    GameObject UnwalkableMaker;

    bool UnwalkableOnWave = false;
    public override void OnEventRaised(Object data)
    {
        if(UnwalkableOnWave)
            UnwalkableMaker.SetActive(false);
    }
}
