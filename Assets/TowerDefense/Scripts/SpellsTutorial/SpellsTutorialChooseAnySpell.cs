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
        GameObject IceSphere = GameObject.Find("IceNode 1(Clone)");
        GameObject FireSphere = GameObject.Find("FireNode 1(Clone)");
        GameObject ElectricSphere = GameObject.Find("ElectricNode 1(Clone)");

        if (IceSphere == null)
        {
            Debug.LogError("NIE ZNALEZIONO LODOWEGO ŻYWIOŁU");
            return false;
        }
        if (FireSphere == null)
        {
            Debug.LogError("NIE ZNALEZIONO OGNISTEGO ŻYWIOŁU");
            return false;
        }
        if (ElectricSphere == null)
        {
            Debug.LogError("NIE ZNALEZIONO ELEKTRYCZNEGO ŻYWIOŁU");
            return false;
        }

        IceSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveIce);
        FireSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveFire);
        FireSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveElectric);
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

    public void OnActiveElectric()
    {
        SpellsTutorialMainNode.Instance.isElementChose = true;
        SpellsTutorialMainNode.Instance.elementChose = ElementType.electricity;
        SetNextStep();
    }
}
