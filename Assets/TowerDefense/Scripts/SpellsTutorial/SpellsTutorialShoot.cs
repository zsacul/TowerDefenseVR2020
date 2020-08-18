using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialShoot : SpellsTutorialSubnode
{
    [SerializeField]
    MagicManager MM;

    public override void EnterStep()
    {
        base.EnterStep();
        MM.onDeactivate.AddListener(SetNextStep);
    }
}
