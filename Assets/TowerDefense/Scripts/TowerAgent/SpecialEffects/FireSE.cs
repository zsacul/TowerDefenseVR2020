using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FireSE : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;

    public UnityEvent start, end;

    private void Start()
    {
        type = ElementType.fire;
        currentSpecialTime = 0;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        //TODO - wizualizacja podpalenia

        // currentSpecialTime += time;
        currentSpecialTime = 0;

        if (currentSpecialTime < time)
        {
            start.Invoke();
            while (currentSpecialTime < time && enemy != null)
            {
                yield return new WaitForSeconds(0.3f);
                enemy.ApplyDamage(dmg);
                currentSpecialTime += 0.3f;
            }

            if (enemy != null)
            {
                end.Invoke();
                //TODO - koniec wizualizacji podpalenia
            }
        }
    }
}
