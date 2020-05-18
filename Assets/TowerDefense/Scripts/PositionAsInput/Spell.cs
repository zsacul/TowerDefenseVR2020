using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell", order = 4)]
public class Spell : ScriptableObject
{
    public GameObject spellPrefab;
    public Vector2Int[] castSequence;
    public IChargable Cast(Vector3 position, Quaternion direction, float charge, Transform parent)
    {
        GameObject o = Instantiate(spellPrefab, position, direction, parent);
        IChargable c = o.GetComponent<IChargable>();
        c.SetCharge(charge);
        return c;
    }
}
