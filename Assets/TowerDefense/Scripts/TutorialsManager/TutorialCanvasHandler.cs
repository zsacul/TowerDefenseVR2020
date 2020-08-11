using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasHandler : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        Destroy(gameObject);
    }
}
