using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public int fireUpgradeCost;
    public int iceUpgradeCost;
    public int lightningUpgradeCost;
    public int windUpgradeCost;

    private GameObject thisChunk;
    private BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        thisChunk = transform.parent.gameObject;
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    public void ProceedTowerUpgrade(TowerType towerType)
    {
        int upgradeCost;
        switch(towerType)
        {
            case TowerType.fire:
                upgradeCost = fireUpgradeCost;
                break;
            case TowerType.ice:
                upgradeCost = iceUpgradeCost;
                break;
            case TowerType.lightning:
                upgradeCost = lightningUpgradeCost;
                break;
            case TowerType.wind:
                upgradeCost = windUpgradeCost;
                break;
            default:
                upgradeCost = 10000000;
                Debug.Log("UpgradeTower: ProceedTowerUpgrade(): Couldn't get upgradeCost");
                break;
        }

        if (buildManager.GetMoney() >= upgradeCost)
        {
            Debug.Log("UpgradeSuccess");
            //gradeSuccess.Raise();
            buildManager.DecreaseMoney(upgradeCost);
            thisChunk.GetComponent<Chunk>().UpgradeTower(towerType);
        }
        else
        {
            //UpgradeFailure.Raise();
        }
    }
}
