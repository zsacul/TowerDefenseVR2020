using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutActivatePointer : TeleportSubNode
{
    // Start is called before the first frame update
    void Start()
    {
        doneInPast = false;
        state = false;
        gameObject.SetActive(false);
    }

    public override void SetPrevStep()
    {
        if(!TeleportMainNode.Instance.teleDone)
            base.SetPrevStep();
    }

    public override void SetNextStep()
    {
        if(!TeleportMainNode.Instance.teleDone)
            base.SetNextStep();
    }
}
