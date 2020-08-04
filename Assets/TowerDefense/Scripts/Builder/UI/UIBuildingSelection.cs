using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBuildingSelection : MonoBehaviour
{
    [SerializeField]
    private GameEvent UIBuildingClicked;
    [SerializeField]
    private UnityEvent OnClick;

    private BuildManager buildManager;
    private bool collided;

    void Start()
    {
        collided = false;
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collided with UI Tower. gameObject that collider = " + other.gameObject.name);
        if (other.gameObject.tag == "HandCollider" && !collided)
        {
            //UIBuildingClicked.Raise();
            OnClick.Invoke();
            buildManager.UITowerClicked();
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
