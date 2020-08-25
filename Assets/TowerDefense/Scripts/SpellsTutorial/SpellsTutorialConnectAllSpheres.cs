using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialConnectAllSpheres : SpellsTutorialSubnode
{
    GameObject lastSphere;
    public override void EnterStep()
    {
        base.EnterStep();
        StartCoroutine(findLastElementNode());
    }

    IEnumerator findLastElementNode()
    {
        yield return new WaitForSeconds(0.5f);
        switch (SpellsTutorialMainNode.Instance.elementChose)
        {
            case ElementType.ice:
                {
                    lastSphere = GameObject.Find("IceNode 2(Clone)");
                    if (lastSphere == null)
                        Debug.LogError("NIE MA IceNode2 (Clone) NA MAPIE");
                    break;
                }
            case ElementType.fire:
                {
                    lastSphere = GameObject.Find("FireNode 2(Clone)");
                    if (lastSphere == null)
                        Debug.LogError("NIE MA FireNode 2(Clone) NA MAPIE");
                    break;
                }
            case ElementType.electricity:
                {
                    lastSphere = GameObject.Find("ElectricNode 2(Clone)");
                    if (lastSphere == null)
                        Debug.LogError("NIE MA ElectricNode 2(Clone) NA MAPIE");
                    break;
                }
            default:
                {
                    Debug.LogError("Wybrano żywioł nieuwzględniony");
                    break;
                }
        }
        lastSphere.GetComponent<UINode>().onSelect.AddListener(SetNextStep);
    }
}
