using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBuildingSelection : MonoBehaviour
{
    //[SerializeField]
    //private GameEvent UIBuildingClicked;
    //[SerializeField]
    //private UnityEvent OnClick;

    private BuildManager buildManager;
    private UIBuildings uiBuildings;
    private bool collided;

    void Start()
    {
        collided = false;
        StartCoroutine(waitForGameManager());
    }

    IEnumerator waitForGameManager()
    {
        yield return new WaitForSeconds(0.1f);
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        uiBuildings = GameObject.Find("UI").GetComponent<UIBuildings>();
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collided with UI Tower. gameObject that collider = " + other.gameObject.name);
        if (other.gameObject.tag == "HandCollider" && !collided)
        {
            if(buildManager == null)
            {
                buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
            }
            //OnClick.Invoke();

            if(tag == "UITowerSelection")
            {
                buildManager.UITowerClicked();
            }
            
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "HandCollider")
        {
            collided = false;
        }

        if(tag == "UICancelSelection")
        {
            buildManager.ChooseNone();
        }
    }
}
