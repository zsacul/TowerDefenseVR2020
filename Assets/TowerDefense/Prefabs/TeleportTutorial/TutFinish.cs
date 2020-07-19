﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutFinish : TeleportSubNode
{
    [SerializeField]
    GameObject teleportDest;
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    public override void EnterStep()
    {
        base.EnterStep();
        teleportDest.SetActive(false);
        TeleportMainNode.Instance.SetTutorialDone();
    }

    public override void ExitStep()
    {
        base.ExitStep();
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
    }
}
