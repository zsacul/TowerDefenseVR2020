using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class IceSE1 : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;

    public UnityEvent start, end;

    private void Start()
    {
        type = ElementType.ice;
        currentSpecialTime = 0f;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        //TODO - Wizualizacja zamrozenia

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        currentSpecialTime += time;

        if (currentSpecialTime == time)
        {
            start.Invoke();
            while (currentSpecialTime > 0 && agent != null)
            {
                yield return new WaitForSeconds(0.3f);
                enemy.ApplyDamage(dmg);
                currentSpecialTime += Time.deltaTime;
            }

            if (agent != null)
            {
                agent.isStopped = false;
                end.Invoke();
                //TODO - koniec wizualizacji zamrozenia
            }
        }
    }
  }
