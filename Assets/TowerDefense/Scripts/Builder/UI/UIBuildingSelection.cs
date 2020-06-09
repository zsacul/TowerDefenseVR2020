using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingSelection : MonoBehaviour
{
    [SerializeField]
    private GameEvent UIBuildingClicked;
    private bool collided;

    void Start()
    {
        collided = false;
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collided with UI Tower. gameObject that collider = " + other.gameObject.name);
        if (other.gameObject.tag == "HandCollider" && !collided)
        {
            UIBuildingClicked.Raise();
            collided = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "HandCollider")
        {
            collided = false;
        }
    }
}
