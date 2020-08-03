using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowPractice : CrossbowSubNode
{

    [SerializeField]
    GameObject ArcheryTarget;
    [SerializeField]
    int RequiredNumOfHits;
    int numOfHits;
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
        numOfHits = 0;
    }
    
    public void Hit()
    {
        numOfHits++;
        if (numOfHits == RequiredNumOfHits)
            SetNextStep();
    }

    public override void EnterStep()
    {
        base.EnterStep();
        ArcheryTarget.SetActive(true);
    }
    // Update is called once per frame
}
