using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : BaseTower
{
    [SerializeField]
    fanRotation fan;
    [SerializeField]
    float windBoostDuration;
    [SerializeField]
    Elements elementsInfo;
    // Start is called before the first frame update
    void Start()
    {
        type = ElementType.wind;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        currentDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(Blow());
            currentDelay = 0f;
        }

    }

    IEnumerator Blow()
    {
        fan.speedupRotation(2);

        foreach (var enemy in enemiesList)
        {
            Resistance res = elementsInfo.GetResistence(enemy.GetComponent<EnemyHPManager>().GetElementType(), ElementType.wind);
            switch (res)
            {
                case Resistance.high:
                    break;
                case Resistance.normal:
                    enemy.GetComponent<EnemyHPManager>().Slowdown(2f);
                    break;
                case Resistance.low:
                    enemy.GetComponent<EnemyHPManager>().Slowdown(3f);
                    break;
                default:
                    break;
            }
        }

        yield return new WaitForSeconds(windBoostDuration);

        foreach (var enemy in enemiesList)
        {
            Resistance res = elementsInfo.GetResistence(enemy.GetComponent<EnemyHPManager>().GetElementType(), ElementType.wind);
            switch (res)
            {
                case Resistance.high:
                    break;
                case Resistance.normal:
                    enemy.GetComponent<EnemyHPManager>().Speedup(2f);
                    break;
                case Resistance.low:
                    enemy.GetComponent<EnemyHPManager>().Speedup(3f);
                    break;
                default:
                    break;
            }
        }

        fan.speedupRotation(0.5f);
    }
}
