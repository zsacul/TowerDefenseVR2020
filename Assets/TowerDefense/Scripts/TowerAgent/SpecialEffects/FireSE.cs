using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireSE : MonoBehaviour
{
    int currentNumOfHits;
    ElementType type;

    private void Start()
    {
        type = ElementType.fire;
        currentNumOfHits = 0;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, int time)
    {
        Debug.Log("jestesmy");

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
    }
}
