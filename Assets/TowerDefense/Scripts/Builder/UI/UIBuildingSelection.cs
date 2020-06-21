using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingSelection : MonoBehaviour
{
    [SerializeField]
    private GameEvent UIBuildingClicked;

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collided with UI Tower. gameObject that collider = " + other.gameObject.name);
        if (other.gameObject.tag == "HandCollider")
        {
            Debug.Log("Raise " + UIBuildingClicked.name);
            UIBuildingClicked.Raise();
        }
    }
}
