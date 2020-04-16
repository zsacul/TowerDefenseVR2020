using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower: BaseTower
{
    [SerializeField]
    GameObject prefTowerFire;
    [SerializeField]
    GameObject prefTowerStone;
    [SerializeField]
    GameObject prefTowerElectric;
    [SerializeField]
    GameObject prefTowerIce;
    [SerializeField]
    GameObject prefTowerEarth;
    [SerializeField]
    GameObject prefTowerWind;
    [SerializeField]
    GameObject prefStoneBullet;

    void Start()
    {
        type = ElementType.stone;
        bulletPref = prefStoneBullet;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        gunsList = new List<Gun>(GetComponentsInChildren<Gun>());
        setBulletTypeInGuns();
        maxNumberOfGuns = gunsList.Count;
        currentDelay = 0f;
        numberOfActiveGuns = 1;
        deactivateGuns();
        gunsList[0].gameObject.SetActive(true);
    }
    
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if ( numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(shoot());
            currentDelay = 0f;
        }
    }

    public void ChangeType(ElementType t)
    {
        GameObject instTower;
        switch (t)
        {
            case ElementType.fire:
                instTower = Instantiate(prefTowerFire, transform.position, Quaternion.identity) as GameObject;
                break;
            case ElementType.electricity:
                instTower = Instantiate(prefTowerElectric, transform.position, Quaternion.identity) as GameObject;
                break;
            case ElementType.ice:
                instTower = Instantiate(prefTowerIce, transform.position, Quaternion.identity) as GameObject;
                break;
            case ElementType.earth:
                instTower = Instantiate(prefTowerEarth, transform.position, Quaternion.identity) as GameObject;
                break;
            case ElementType.wind:
                instTower = Instantiate(prefTowerWind, transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                Debug.Log("Bledna proba zmiany typu wiezyczki!");
                return;
        }

        DestroyTower();
    }

    void DestroyTower()
    {
        Destroy(gameObject);
    }

}   