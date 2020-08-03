using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowPractice : CrossbowSubNode
{
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

    // Update is called once per frame
}
