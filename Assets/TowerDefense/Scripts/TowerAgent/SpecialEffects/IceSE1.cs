using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class IceSE1 : MonoBehaviour
{
    float currentSpecialTime;
    ElementType type;
    [SerializeField]
    GameObject SnowPE;
    [SerializeField]
    MaterialColorChanger BoximonModel;
    public UnityEvent start, end;
    bool isActive;

    private void Start()
    {
        type = ElementType.ice;
        currentSpecialTime = 0f;
        isActive = false;
    }

    public IEnumerator RunSpecialEffect(EnemyHPManager enemy, float dmg, float time)
    {
        currentSpecialTime += time;

        if (!isActive)
        {
            isActive = true;
            //GameObject Snow = Instantiate(SnowPE);
            //Snow.transform.position = GetComponentInChildren<EnemyTargetPoint>().transform.position;
            //Snow.transform.SetParent(GetComponentInChildren<EnemyTargetPoint>().transform);
            StartCoroutine(BoximonModel.SetFreezColor());

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            //        agent.isStopped = true;
            start.Invoke();
            while (currentSpecialTime > 0f && agent != null)
            {
                Debug.Log("czas mrozenia = " + currentSpecialTime);
                yield return new WaitForSeconds(0.3f);
                enemy.ApplyDamage(dmg);
                currentSpecialTime -= 0.3f;
            }

            isActive = false;

            if (agent != null)
            {
//                agent.isStopped = false;
                end.Invoke();
                //Destroy(Snow);
                StartCoroutine(BoximonModel.SetNormalColor());
            }
        }
    }
  }
