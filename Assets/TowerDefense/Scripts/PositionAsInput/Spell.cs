using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell", order = 4)]
public class Spell : ScriptableObject
{
    public GameObject spellPrefab;
    public Vector3 offset;
    public Vector2Int[] castSequence;
    public IChargable Cast(Vector3 position, Quaternion direction, float charge, Transform parent)
    {
        GameObject o = Instantiate(spellPrefab, position + offset, direction, parent);
        o.transform.localPosition = offset;
        IChargable c = o.GetComponent<IChargable>();
        c.SetCharge(charge);
        return c;
    }
}
