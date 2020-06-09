using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITowerClickedListener : GameEventListener
{
    private UIBuildings uiBuildings;
    private void Start()
    {
        uiBuildings = transform.gameObject.GetComponent<UIBuildings>();   
    }

    // If UITower was clicked, UITowerClicked event was raised and we call UIBuildings.OnUITowerClicked()
    public override void OnEventRaised(Object data)
    {
        //Debug.Log("UITowerSelectedListener calls OnUITowerCliked()");
        uiBuildings.OnUITowerClicked();
    }
}
