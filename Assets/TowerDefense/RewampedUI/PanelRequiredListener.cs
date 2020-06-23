using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRequiredListener : GameEventListener
{

    public override void OnEventRaised(Object data)
    {
        //Debug.Log("I AM CALLED!");
        gameObject.SetActive(true);
        transform.GetComponent<GenericNBoxSelector>().Respawn();
    }
}

