
#define DEBUG
//#undef DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
    // Start is called before the first frame update
    void Start()
    {
        type = ElementType.wind;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        currentDelay = 0f;
        currentSpecialEffectDuration = 0f;
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isBoosActive = false;
    }

    // Update is called once per frame
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
        enemy.GetComponent<Rigidbody>().freezeRotation = false;
        enemy.GetComponent<NavMeshAgent>().isStopped = false;
        /*
        switch (res)
        {
            case Resistance.high:
                Debug.Log("high");
                enemy.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * (dot * -1)/2f);
                break;
            case Resistance.normal:
                Debug.Log("normal");
                enemy.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * (dot * -1));
                break;
            case Resistance.low:
                Debug.Log("low");
                //enemy.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * (dot * -1) * 2);
                Debug.Log(ExplosionSource.transform.position);
                break;
        }
        */

    }
    
}
