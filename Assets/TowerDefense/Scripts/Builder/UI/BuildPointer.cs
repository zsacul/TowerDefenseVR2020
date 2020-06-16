using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.CameraRig.UnityXRCameraRig.Input;


public class BuildPointer : GameEventListener
{
    public GameObject PointerActivationEvent;

    private PointerAction pointerAction;

    public override void OnEventRaised(Object data)
    {
        pointerAction.ChangeValue();
    }

    void Start()
    {
        pointerAction = PointerActivationEvent.GetComponent<PointerAction>();
    }


}
