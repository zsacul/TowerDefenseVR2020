using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialActivateSpellPanel : SpellsTutorialSubnode
{
    public void SpellTriggerRelease()
    {
        if (!SpellsTutorialMainNode.Instance.isElementChose)
        {
            SetPrevStep();
        }
    }
}
