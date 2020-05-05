using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell", order = 4)]
public class Spell : ScriptableObject
{
    public GameObject spellPrefab;
    public Vector2Int[] castSequence;
    public void Cast(Vector3 position, Quaternion direction, float charge)
    {
        Instantiate(spellPrefab, position, direction);
    }
}
