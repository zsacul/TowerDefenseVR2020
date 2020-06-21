using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricitySE : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;
    public UnityEvent start, end;
    private void Start()
    {
        type = ElementType.electricity;
        currentSpecialTime = 0f;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        currentSpecialTime += time;

        if (currentSpecialTime == time)
        {
            start.Invoke();
            // TODO - stworz chmurke
            while (currentSpecialTime > 0 && enemy != null)
            {
                Debug.Log("ilosc wykonanych obrazen" + currentSpecialTime);
                enemy.ApplyDamage(dmg);
                currentSpecialTime += Time.deltaTime;
                yield return new WaitForSeconds(0.3f);
            }
            if (enemy != null)
            {
                end.Invoke();
                //TODO - zniszcz chmurke
            }
        }
    }
}
