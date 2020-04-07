using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceSE1 : MonoBehaviour
{
    int currentNumOfHits;
    ElementType type;
    private void Start()
    {
        type = ElementType.ice;
        currentNumOfHits = 0;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, int time)
    {
        Debug.Log("jestesmy");
        Vector3 tmpDest = enemy.GetComponent<NavMeshAgent>().destination;
        enemy.GetComponent<NavMeshAgent>().destination = enemy.gameObject.transform.position;

        if (currentNumOfHits > 0)
            currentNumOfHits -= time;
        else
        {
            while (currentNumOfHits < time)
            {
                Debug.Log("ilosc wykonanych obrazen" + currentNumOfHits);
                yield return new WaitForSeconds(0.3f);
                enemy.ApplyDamage(dmg);
                currentNumOfHits++;
            }
            currentNumOfHits = 0;
        }

        enemy.GetComponent<NavMeshAgent>().destination = tmpDest;
    }
}
