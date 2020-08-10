﻿//#define DEBUG
#undef DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ElectricTower : BaseTower
{
    [SerializeField]
    int maxNumberOfEnemiesInChain;
    [SerializeField]
    int upgradeCostIncMaxNumberOfEnemiesInChain;
    [SerializeField]
    float radiusOfFindingNextEnemyInChain;
    [SerializeField]
    GameObject lightning;
    [SerializeField]
    GameObject ligtningMaker;
    [SerializeField]
    GameObject EnemiesTarget;

    List<GameObject> lightningsList = new List<GameObject>();
    List<GameObject> hitedEnemiesList = new List<GameObject>();

    float speedOfShaking = 80.0f; //how fast enemy shakes when shocked
    float amountOfShaking = 5.0f; //how much enemy shakes when shocked

    public UnityEvent makeLightning;
    public int UpgradeCostIncMaxNumberOfEnemiesInChain => upgradeCostIncMaxNumberOfEnemiesInChain;

    void Start()
    {
        EnemiesTarget = FindObjectOfType<EndpointManager>().gameObject;
        type = ElementType.electricity;
        bulletPref = lightning;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
    }

    void Update()
    {
        hitedEnemiesList.RemoveAll(item => item == null || item.GetComponent<Collider>() == null);
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(makeLightningChain(enemiesList[0]));
            currentDelay = 0f;
        }
        if(hitedEnemiesList.Count > 0)
            dealHitedEnemies();
    }

    void dealHitedEnemies(){
        foreach (GameObject e in hitedEnemiesList){
            Vector3 rot = e.transform.rotation.eulerAngles;
            rot.y += Mathf.Sin(Time.time * speedOfShaking) * amountOfShaking;
            e.transform.rotation = Quaternion.Euler(rot);
        }
    }


    IEnumerator  makeLightningChain(GameObject t)
    {
        makeLightning.Invoke();
        GameObject target = t;
        GameObject startPoint = ligtningMaker;
        int numberOfHits = 1;
        
        while (target != null && numberOfHits < maxNumberOfEnemiesInChain)
        {
            if (target.GetComponent<NavMeshAgent>() == null)
                continue;
            target.GetComponent<NavMeshAgent>().isStopped = true;
            createLightning(startPoint, target);
            startPoint = target.GetComponent<EnemyHPManager>().GetTargetPoint();
            numberOfHits++;
            target = findNextEnemy(startPoint.transform.position, radiusOfFindingNextEnemyInChain, hitedEnemiesList);
        }

        yield return new WaitForSeconds(0.8f);
        deleteLightnings();
        deleteHitedEnemyList();
    }

   void deleteHitedEnemyList(){
        int i = 1;
        foreach( GameObject e in hitedEnemiesList){
            e.GetComponent<NavMeshAgent>().isStopped = false;
            e.GetComponent<EnemyHPManager>().ApplyDamage(new Bullet(e, 1, damage / i, ElementType.electricity));
            i++;
        }
        hitedEnemiesList.Clear();
    }

    void createLightning(GameObject start, GameObject end)
    {
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        instLightning.transform.parent = GetComponentInParent<triggerEnemiesCollisionList>().gameObject.transform;
        
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = start;
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = end.GetComponent<EnemyHPManager>().GetTargetPoint();
        
        lightningsList.Add(instLightning);
        hitedEnemiesList.Add(end);
    }

    void deleteLightnings()
    {
        foreach (GameObject g in lightningsList)
            Destroy(g.gameObject);
    }

    GameObject findNextEnemy(Vector3 center, float radius, List<GameObject> hited)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject go = hitColliders[i].gameObject;
            if (go.tag == "Enemy" && !hited.Contains(go))
            {
                return hitColliders[i].gameObject;
            }
            i++;
        }
        return null;
    }

    public void UpgradeLightningLength()
    {
        maxNumberOfEnemiesInChain++;
        upgradeCostIncMaxNumberOfEnemiesInChain = (int)((float)upgradeCostIncMaxNumberOfEnemiesInChain * (1f + upgradeRiseInPercent / 100f));
    }

}
