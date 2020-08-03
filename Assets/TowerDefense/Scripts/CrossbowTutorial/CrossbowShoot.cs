using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowShoot : CrossbowSubNode
{
    [SerializeField]
    int CrossbowId;
    [SerializeField]
    HandDeployer LeftHand;
    [SerializeField]
    HandDeployer RightHand;
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckIfCrossbowIsLoaded();
    }

    void CheckIfCrossbowIsLoaded()
    {
        if (RightHand.listIterator == CrossbowId)
        {
            if (!RightHand.PropList[CrossbowId].Instance.GetComponent<CrossbowManager>().loaded)
                SetNextStep();
        }
        else
            if (!LeftHand.PropList[CrossbowId].Instance.GetComponent<CrossbowManager>().loaded)
            SetNextStep();
    }
}
