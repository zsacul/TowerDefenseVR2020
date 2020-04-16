using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceSE1 : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;
    private void Start()
    {
        type = ElementType.ice;
        currentSpecialTime = 0f;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        Vector3 tmpDest = enemy.GetComponent<NavMeshAgent>().destination;
        enemy.GetComponent<NavMeshAgent>().destination = enemy.gameObject.transform.position;
        
        currentSpecialTime += time;

        if (currentSpecialTime == time)
            while (currentSpecialTime > 0 && agent != null)
            {
                Debug.Log("ilosc wykonanych obrazen" + currentSpecialTime);
                enemy.ApplyDamage(dmg);
                currentSpecialTime += Time.deltaTime;
                yield return new WaitForSeconds(0.3f);
            }   
    }
  }
