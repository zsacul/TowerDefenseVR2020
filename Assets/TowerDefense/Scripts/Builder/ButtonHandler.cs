using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private bool pushed;
    private GameObject buttonInstance;

    private void Start()
    {
        pushed = false;
        buttonInstance = transform.parent.gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            StartCoroutine(TryUpgradingTower());
            transform.localPosition += new Vector3(0f, -0.1f, 0f);
        }
    }

    IEnumerator TryUpgradingTower()
    {
        yield return new WaitForSeconds(1.2f);

        GameObject parentCanvas = buttonInstance.transform.parent.gameObject;

        if (IsUpgradeToElementTower(parentCanvas))
        {
            parentCanvas.GetComponent<TowerUpgrade>().Upgrade();
        }

        else if(IsStatsUpgrade(parentCanvas))
        {
            GameObject elementTower = parentCanvas.transform.parent.gameObject;
            elementTower.GetComponent<StatsUpgradeManager>().TowerLevelUp();
        }

        transform.localPosition += new Vector3(0f, 0.1f, 0f);
        pushed = false;
    }

    private bool IsUpgradeToElementTower(GameObject parentCanvas)
    {
        return parentCanvas.transform.parent.tag == "BaseTower";
    }

    private bool IsStatsUpgrade(GameObject parentCanvas)
    {
        return parentCanvas.transform.parent.tag != "BaseTower";
    }
}
