using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CrossbowGrabString : CrossbowSubNode
{
    [SerializeField]
    int CrossbowStringId = 4;
    [SerializeField]
    int CrossbowId = 3;
    [SerializeField]
    HandDeployer LeftHand;
    [SerializeField]
    HandDeployer RightHand;
    [SerializeField]
    CrossbowDraw draw;
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    bool IsCrossbowStringInHand()
    {
        return (CrossbowStringId == LeftHand.listIterator || CrossbowStringId == RightHand.listIterator);
    }

    bool IsCrossbowLoaded()
    {
        if (RightHand.listIterator == CrossbowId)
        {
            if (RightHand.PropList[CrossbowId].Instance.GetComponent<CrossbowManager>().loaded)
                return true;
        }
        else if (LeftHand.listIterator == CrossbowId)
            if (LeftHand.PropList[CrossbowId].Instance.GetComponent<CrossbowManager>().loaded)
                return true;

        return false;
    }

    public void WeaponChanged()
    {
        if (state)
        {
            if (!IsCrossbowStringInHand() && !IsCrossbowLoaded())
                SetPrevStep();
        }
        else
            if (IsCrossbowStringInHand())
            SetNextStep();
    }

    public override void EnterStep()
    {
        base.EnterStep();
        if (IsCrossbowLoaded())
            SetNextStep();
    }
}
