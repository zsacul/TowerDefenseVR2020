using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialFinished : SpellsTutorialSubnode
{
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    public override void EnterStep()
    {
        base.EnterStep();
        SpellsTutorialMainNode.Instance.SetTutorialDone();
    }

    public override void ExitStep()
    {
        Invoke("Wyjscie", 10f);
        StartCoroutine("Wait");
    }

    void Wyjscie()
    {
        base.ExitStep();
        canvasToDisplayText.enabled = true;
        InfoForPlayer.text = "";
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
    }
}
