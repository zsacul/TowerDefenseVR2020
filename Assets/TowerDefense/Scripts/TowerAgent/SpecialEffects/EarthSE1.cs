using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EarthSE1 : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;

    public UnityEvent start, end;
    private void Start()
    {
        type = ElementType.earth;
        currentSpecialTime = 0f;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        //TODO - Wizualizacja ogluszenia
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        currentSpecialTime += time;

        if (currentSpecialTime == time)
        {
            start.Invoke();
            while (currentSpecialTime > 0 && agent != null)
            {
                Debug.Log("ilosc wykonanych obrazen" + currentSpecialTime);
                enemy.ApplyDamage(dmg);
                currentSpecialTime += Time.deltaTime;
                yield return new WaitForSeconds(0.3f);
            }

            if (agent != null)
            {
                agent.isStopped = false;
                end.Invoke();
                //TODO - koniec wizualizacji ogluszenia
            }
        }
    }
}
