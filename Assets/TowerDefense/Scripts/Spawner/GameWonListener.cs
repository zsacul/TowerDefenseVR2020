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
        text.text = "Congratulations, our castle has been defended!\nGame will restart in 10 seconds.";
        Endgame.Instance.ResetScene();
        Debug.Log("Game Won raised in TutorialCanvas.GameLostListener");
        OnWin.Invoke();
    }
}
