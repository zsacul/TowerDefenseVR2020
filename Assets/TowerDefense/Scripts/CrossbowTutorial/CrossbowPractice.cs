using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrossbowPractice : CrossbowSubNode
{
    [SerializeField]
    TextMeshProUGUI numOfHitsTEXT;
    [SerializeField]
    TextMeshProUGUI requestNumberOfHitsTEXT;
    [SerializeField]
    GameObject ArcheryTarget;
    [SerializeField]
    int RequiredNumOfHits;
    [NonSerialized]
    public int numOfHits;
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
        numOfHits = 0;
        requestNumberOfHitsTEXT.text = RequiredNumOfHits.ToString();
        numOfHitsTEXT.text = numOfHits.ToString();
    }
    
    public void Hit()
    {
        numOfHits++;
        numOfHitsTEXT.text = numOfHits.ToString();
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
