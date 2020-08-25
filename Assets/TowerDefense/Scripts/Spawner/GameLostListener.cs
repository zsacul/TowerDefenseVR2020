using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameLostListener : GameEventListener
{
    public UnityEvent OnLost;
    public TextMeshProUGUI text;

    public override void OnEventRaised(Object data)
    {
        text.text = "Enemy has taken your castle, you have lost!\nGame will restart in 10 seconds.";
        Endgame.Instance.ResetScene();
        Debug.Log("Game Lost raised in TutorialCanvas.GameLostListener");
        OnLost.Invoke();
    }
}
