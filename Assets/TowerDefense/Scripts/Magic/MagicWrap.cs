using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicWrap", menuName = "ScriptableObjects/MagicWrap", order = 7)]
public class MagicWrap : ScriptableObject
{
    public void Cast(int index)
    {
        MagicManager.Cast(index);
    }
    public void SetCharge(float charge)
    {
        MagicManager.SetCharge(charge);
    }
    public void Release()
    {
        MagicManager.Release();
    }
}
