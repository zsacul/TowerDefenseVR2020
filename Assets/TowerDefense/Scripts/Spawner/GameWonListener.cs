using SpawnManaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameWonListener : GameEventListener
{
    public UnityEvent OnWin;
    public TextMeshProUGUI text;

    public override void OnEventRaised(Object data)
    {
        text.text = "Congratulations! You have won!";
        OnWin.Invoke();
    }
}
