using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Cast;

public class TutTeleport : TeleportSubNode
{
    [SerializeField]
    GameObject Target;
    [SerializeField]
    ParabolicLineCast Pointer;

    private void Start()
    {
        doneInPast = false;
        state = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (TeleportMainNode.Instance.GetCurrentTutorial() == this)
        {
            if (Vector3.Distance(new Vector3(Camera.main.transform.position.x, Target.transform.position.y, Camera.main.transform.position.z), Target.transform.position) <= 2f)
            {
                TeleportMainNode.Instance.teleDone = true;
                SetNextStep();
            }

            if ((Vector3.Distance(Pointer.PointingPosition, Target.transform.position) > 0.5f) && !TeleportMainNode.Instance.GetState() && !TeleportMainNode.Instance.teleDone)
                SetPrevStep();
        }
    }

    public override void ExitStep()
    {
        Debug.Log("ExitStep()");
        base.ExitStep();
        if (TeleportMainNode.Instance.teleDone)
        {
            Debug.Log("inside if in ExitStep()");
            TeleportMainNode.Instance.ClearTasks();
            TeleportMainNode.Instance.crossbow.SetActive(true);
            Target.SetActive(false);
        }
    }
}
