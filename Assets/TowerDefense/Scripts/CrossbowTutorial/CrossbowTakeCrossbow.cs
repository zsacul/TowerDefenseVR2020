using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTakeCrossbow : CrossbowSubNode
{
    [SerializeField]
    int CrossbowId;
    [SerializeField]
    HandDeployer LeftHand;
    [SerializeField]
    HandDeployer RighHand;
    [SerializeField]
    GameObject Target;
    void Start() 
    {
        state = false;
        gameObject.SetActive(false);
    }

    public override void EnterStep()
    {
        base.EnterStep();
        Target.SetActive(true);
    }

    bool IsWeaponCrossbow()
    {
        return (CrossbowId == LeftHand.listIterator || CrossbowId == RighHand.listIterator);
    }

    public void WeaponChanged()
    {
        Debug.Log("lewa: " + LeftHand.listIterator + " prawa:" + RighHand.listIterator);

        if (state)
        {
            if (!IsWeaponCrossbow())
            {
                Debug.Log("wyrzucono kusze");
                SetPrevStep();
            }

        }
        else
            if (IsWeaponCrossbow())
        {
            Debug.Log("W rece jest kusza");
            SetNextStep();
        }
    }
}
