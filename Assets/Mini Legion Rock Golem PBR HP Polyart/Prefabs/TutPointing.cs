using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Cast;
public class TutPointing : TeleportSubNode
{
    [SerializeField]
    GameObject Target;
    [SerializeField]
    ParabolicLineCast Pointer;
    // Start is called before the first frame update
    void Start()
    {
        doneInPast = false;
        state = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Target == null || Pointer == null)
        {
            Debug.Log("Target lub Pointer jest nullem");
            return;
        }
        if (Vector3.Distance(Pointer.PointingPosition, Target.transform.position) < 0.5f)
        {
            SetNextStep();
        }
    }

    public override void EnterStep()
    {
        base.EnterStep();
        if(!Target.activeSelf)
            Target.SetActive(true);
    }
}
