using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : BaseTower
{
    [SerializeField]
    GameObject FireBullet;
    // Start is called before the first frame update
    void Start()
    {
        type = ElementType.fire;
        bulletPref = FireBullet;
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
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(shoot());
            currentDelay = 0f;
        }
    }

}