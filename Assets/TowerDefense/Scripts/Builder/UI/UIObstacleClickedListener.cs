using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObstacleClickedListener : GameEventListener
{
    private UIBuildings uiBuildings;
    private void Start()
    {
        uiBuildings = transform.gameObject.GetComponent<UIBuildings>();
    }

    public override void OnEventRaised(Object data)
    {
        //uiBuildings.OnUIObstacleClicked();
    }
}
