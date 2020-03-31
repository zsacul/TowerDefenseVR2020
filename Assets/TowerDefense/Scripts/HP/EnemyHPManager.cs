using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHPManager : MonoBehaviour {

    bool isSpecialEffectActive;
    float specialEffectDuration;
    float specialEffectDmg;
    UnityEngine.AI.NavMeshAgent enemyAgent;

    public UnityEvent damaged;
    public UnityEvent killed;
    [Min(1f)]
    public float enemyHP = 100f;

    [SerializeField]
    Elements elementsInfo;
    [SerializeField]
    GameObject targetPoint;
    [SerializeField]
    ElementType type;
    [SerializeField]
    float speed;
    
    
    

    private void Start()
    {
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isSpecialEffectActive = false;
        GetComponent<HealthBar>().SetMaxHp(enemyHP);
    }

    private void Update()
    {
        if (isSpecialEffectActive)
             StartCoroutine(specialEffect());

    }

    private void Death() {
        killed.Invoke();
        Debug.Log("Enemy killed");
        Destroy(enemyAgent);
        Destroy(GetComponent<Collider>());
        Destroy(gameObject, 3);
    }

    public void ApplyDamage(Bullet b) {
        damaged.Invoke();
        Resistance res = elementsInfo.GetResistence(type, b.GetBulletType());
        switch (res)
        {
            case Resistance.high:
                enemyHP -= b.GetDamage() * 0.7f;
                break;
            case Resistance.normal:
                enemyHP -= b.GetDamage();
                isSpecialEffectActive = true;
                specialEffectDuration = b.GetSpecialEffectDuration();
                specialEffectDmg = b.GetSpecialEffectDmg();
                break;
            case Resistance.low:
                enemyHP -= b.GetDamage() * 1.5f;
                isSpecialEffectActive = true;
                specialEffectDuration = b.GetSpecialEffectDuration() * 1.5f;
                specialEffectDmg = b.GetSpecialEffectDmg() * 1.5f;
                break;
            default:
                break;
        };
        
        GetComponent<HealthBar>().updateBar(enemyHP);

        b.SetReadyToDestroy();


        if (enemyHP <= 0 ) {
            Death();
        }


    }

    IEnumerator specialEffect()
    {
        while (specialEffectDuration > 0)
        {
            specialEffectDuration -= Time.deltaTime;
            ApplyDamage(specialEffectDmg);
            yield return new WaitForSeconds(0.3f);
        }

        isSpecialEffectActive = false;
    }

    public void ApplyDamage(float damage)
    {
        damaged.Invoke();
        enemyHP -= damage;

        if (enemyHP <= 0)
        {
            Death();
        }

        GetComponent<HealthBar>().updateBar(enemyHP);

    }

    public GameObject GetTargetPoint(){
        return targetPoint;
    }
}