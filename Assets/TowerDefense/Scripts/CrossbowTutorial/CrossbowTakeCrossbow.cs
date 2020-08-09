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
    void Start() 
    {
        state = false;
        gameObject.SetActive(false);
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
            if (IsWeaponCrossbow() && this.gameObject.activeSelf)
        {
            Debug.Log("W rece jest kusza");
            SetNextStep();
        }
    }

    private void Update()
    {
        Debug.Log("lala");
    }
}
