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

    void FindElementSpheres()
    {
        GameObject IceSphere = GameObject.Find("IceNode 1(Clone)");
        GameObject FireSphere = GameObject.Find("FireNode 1(Clone)");
        GameObject ElectricSphere = GameObject.Find("ElectricNode 1(Clone)");

        if (IceSphere == null)
        {
            Debug.Log("NIE ZNALEZIONO LODOWEGO ŻYWIOŁU lub teleport skipnięty");
        }
        else
            IceSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveIce);

        if (FireSphere == null)
        {
            Debug.Log("NIE ZNALEZIONO OGNISTEGO ŻYWIOŁU lub teleport skipnięty");
        }
        else
            FireSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveFire);

        if (ElectricSphere == null)
        {
            Debug.Log("NIE ZNALEZIONO ELEKTRYCZNEGO ŻYWIOŁU lub teleport skipnięty");
        }
        else
            ElectricSphere.GetComponent<UINode>().onSelect.AddListener(OnActiveElectric);
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
