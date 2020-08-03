using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowFinish : CrossbowSubNode
{
    [SerializeField]
    GameObject ArcheryTarget;
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    public override void EnterStep()
    {
        base.EnterStep();
        ArcheryTarget.SetActive(false);
        CrossbowMainNode.Instance.SetTutorialDone();
    }

    public override void ExitStep()
    {
        Invoke("Wyjscie", 10f);
        StartCoroutine("Wait");
    }

    void Wyjscie()
    {
        base.ExitStep();
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
    }
}
