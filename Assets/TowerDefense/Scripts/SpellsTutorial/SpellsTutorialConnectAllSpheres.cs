using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialConnectAllSpheres : SpellsTutorialSubnode
{
    GameObject lastSphere;
    public override void EnterStep()
    {
        base.EnterStep();
        findLastElementNode();
    }

    bool findLastElementNode()
    {
        switch (SpellsTutorialMainNode.Instance.elementChose)
        {
            case ElementType.ice:
                {
                    lastSphere = GameObject.Find("iceNode3clone(1)");
                    break;
                }
            case ElementType.fire:
                {
                    lastSphere = GameObject.Find("fireNode5clone(1)");
                    break;
                }
            default:
                {
                    Debug.LogError("Wybrano żywioł nieuwzględniony");
                    return false;
                }
        }
        lastSphere.GetComponent<UINode>().onSelect.AddListener(SetNextStep);
        return true;
    }
}
