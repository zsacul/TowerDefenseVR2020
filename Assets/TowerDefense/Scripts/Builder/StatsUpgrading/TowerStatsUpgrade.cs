using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStatsUpgrade : MonoBehaviour
{
    //Indicates if this upgrade was selected by the player
    private bool selected;
    private StatsUpgradeManager statsUpgradeManager;

    void Start()
    {
        statsUpgradeManager = GetComponentInParent<StatsUpgradeManager>();
        selected = false;
    }

    void Update()
    {
        if (selected && Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Upgraded!");
            statsUpgradeManager.TowerLevelUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            statsUpgradeManager.Selected();
        }
    }

    public void setSelectedFalse()
    {
        selected = false;
    }

    public void setSelectedTrue()
    {
        selected = true;
    }
}
