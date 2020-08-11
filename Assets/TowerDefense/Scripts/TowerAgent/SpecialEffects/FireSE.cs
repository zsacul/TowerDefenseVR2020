using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FireSE : MonoBehaviour
{
    [SerializeField]
    GameObject BurningPE;
    float currentSpecialTime;
    ElementType type;

    public UnityEvent start, end;

    private void Start()
    {
        type = ElementType.fire;
        currentSpecialTime = 0f;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        currentSpecialTime += time;
        
        if (currentSpecialTime == time)
        {
            GameObject burning = Instantiate(BurningPE);
            burning.transform.position = GetComponentInChildren<EnemyTargetPoint>().transform.position;
            burning.transform.SetParent(GetComponentInChildren<EnemyTargetPoint>().transform);

            start.Invoke();
            while (currentSpecialTime > 0 && enemy != null)
            {
                yield return new WaitForSeconds(0.3f);
                enemy.ApplyDamage(dmg);
                currentSpecialTime -= 0.3f;
            }

            if (enemy != null)
            {
                end.Invoke();
                Destroy(burning);
                //TODO - koniec wizualizacji podpalenia
            }
        }
    }
}
