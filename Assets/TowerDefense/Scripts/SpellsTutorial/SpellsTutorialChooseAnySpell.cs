using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsTutorialChooseAnySpell : SpellsTutorialSubnode
{
    public override void EnterStep()
    {
        base.EnterStep();
        FindElementSpheres();
    }

    bool FindElementSpheres()
    {
        GameObject IceSphere = GameObject.Find("IceNodeClone(1)");
 //       GameObject FireSphere = GameObject.Find("FireNodeClone(1)");

        if (IceSphere == null)// || FireSphere == null)
        {
            Debug.LogError("NIE ZNALEZIONO WSZYSTKICH WYBORÓW ŻYWIOŁU");
            return false;
        }

        IceSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveIce);
 //       FireSphere.GetComponen<UINode>().onSelect.AddListener(OnActiveFire);

        return true;
    }

    public void OnActiveIce()
    {
        SpellsTutorialMainNode.Instance.isElementChose = true;
        SpellsTutorialMainNode.Instance.elementChose = ElementType.ice;
        SetNextStep();
    }

    public void OnActiveFire()
    {
        SpellsTutorialMainNode.Instance.isElementChose = true;
        SpellsTutorialMainNode.Instance.elementChose = ElementType.fire;
        SetNextStep();
    }
}
