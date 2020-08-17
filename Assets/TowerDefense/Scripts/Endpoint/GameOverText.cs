using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : GameEventListener
{
    public override void OnEventRaised(Object data)
    {
        GetComponent<TextMeshProUGUI>().text = "GAME OVER";
    }
}
