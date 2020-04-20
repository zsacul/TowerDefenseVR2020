
#define DEBUG
//#undef DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class WindTower : BaseTower
{
    [SerializeField]
    fanRotation fan;
    [SerializeField]
    float windBlowDuration;
    [SerializeField]
    float windBlowRotationMult;
    [SerializeField]
    Elements elementsInfo;
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float explosionRadius;
    [SerializeField]
    float explosionUp;
    [SerializeField]
    GameObject ExplosionSource;
    [SerializeField]
    float minDistance;

    float currentSpecialEffectDuration;
    bool isBoosActive;

    public UnityEvent windBlow;
    void Start()
    {
        type = ElementType.wind;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        currentDelay = 0f;
        currentSpecialEffectDuration = 0f;
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isBoosActive = false;
    }

    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;

        if ((numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay) || isBoosActive)
        {
            if (isBoosActive == false && enemyCloseEnought())
            {
                Blow();
                isBoosActive = true;
                fan.speedupRotation(windBlowRotationMult);
            }

            if (currentSpecialEffectDuration > windBlowDuration)
            {
                currentDelay = 0f;
                currentSpecialEffectDuration = 0f;
                isBoosActive = false;
                fan.speedupRotation(1f / windBlowRotationMult);
            }
            else
            {
                currentSpecialEffectDuration += Time.deltaTime;
            }
        }

    }

    bool enemyCloseEnought()
    {
        foreach(var e in enemiesList)
        {
            if (Vector3.Distance(ExplosionSource.transform.position, e.transform.position) < minDistance)
                return true;
        }

        return false;
    }

    void Blow()
    {
        windBlow.Invoke();

        foreach (var enemy in enemiesList)
        {
            var e = enemy.GetComponent<EnemyHPManager>();

            if (e == null)
                continue;

            StartCoroutine(moveBack(enemy));
        }
    }
        
    IEnumerator moveBack(GameObject enemy)
    {
        enemy.GetComponent<Rigidbody>().freezeRotation = true;
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, ExplosionSource.transform.position, explosionRadius, explosionUp, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        if (enemy.GetComponent<NavMeshAgent>() != null && enemy.GetComponent<Rigidbody>() != null)
        {
            enemy.GetComponent<Rigidbody>().freezeRotation = false;
            enemy.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }
    
}
