using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStatsUpgrade : MonoBehaviour
{
    private bool selected;
    private StatsUpgradeManager statsUpgradeManager;

    void Start()
    {
        statsUpgradeManager = GetComponentInParent<StatsUpgradeManager>();
        selected = false;
    }

    void Update()
    {
        if (selected && (Input.GetKeyDown(KeyCode.U)))
        {
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
