using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower: BaseTower
{
    [SerializeField]
    GameObject PrefFireTower;
    [SerializeField]
    GameObject PrefStoneTower;
    [SerializeField]
    GameObject PrefElectricTower;

    [SerializeField]
    GameObject StoneBullet;
    // Start is called before the first frame update
    void Start()
    {
        type = TowerType.stone;
        bulletPref = StoneBullet;
        upgradeRise = 2;
        upgradeCost = 10;
        lvl = 1;
        maxlvl = 4;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        gunsList = new List<Gun>(GetComponentsInChildren<Gun>());
        setBulletTypeInGuns();
        maxNumberOfGuns = gunsList.Count;
        currentDelay = 0f;
        numberOfActiveGuns = 1;
        deactivateGuns();
        gunsList[0].gameObject.SetActive(true);
    }

    

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if ( numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(shoot());
            currentDelay = 0f;
            Upgrade();
            Debug.Log("level:" + lvl);
            if (lvl == 3)
            {
                ChangeType(TowerType.electricity);
                Debug.Log("Zmiana typu na electric");
            }
        }
    }

    public void ChangeType(TowerType t)
    {
        GameObject instTower;
        switch (t)
        {
            case TowerType.fire:
                instTower = Instantiate(PrefFireTower, transform.position, Quaternion.identity) as GameObject;
                break;
            case TowerType.electricity:
                instTower = Instantiate(PrefElectricTower, transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                break;
        }

        DestroyTower();
    }

    void DestroyTower()
    {
        Destroy(gameObject);
    }

}   