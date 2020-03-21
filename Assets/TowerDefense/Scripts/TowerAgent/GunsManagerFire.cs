using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsManagerFire : GunsManagerBase
{
    [SerializeField]
    GameObject FireBullet;
    // Start is called before the first frame update
    void Start()
    {
        type = TowerType.fire;
        upgradeRise = 50;
        upgradeDamageCost = 10;
        upgradeNewGunCost = 10;
        upgradeRangeCost = 10;
        maxDamage = 100;
        maxRadius = 6f;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        gunsList = new List<Gun>(GetComponentsInChildren<Gun>());
        setBulletTypeInGuns();
        maxNumberOfGuns = gunsList.Count;
        currentDelay = 0f;
        numberOfActiveGuns = 1;
        deactivateGuns();
        gunsList[0].gameObject.SetActive(true);
    }

    void setBulletTypeInGuns()
    {
        foreach (Gun g in gunsList)
            g.SetBullet(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(shoot());
            currentDelay = 0f;
            UpgradeAddGun();
        }
    }

}