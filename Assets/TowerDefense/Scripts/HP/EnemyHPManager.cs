using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EnemyHPManager : MonoBehaviour {
    float speedManipulations;
    bool isSpecialEffectActive;
    int specialEffectDurationInSec;
    int specialEffectDmgPerSec;
    ElementType bulletType;
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
        speedManipulations = 1f;
        enemyAgent = GetComponent<NavMeshAgent>();
        //enemyAgent.enabled = false;
        Debug.Log("agent to: " + enemyAgent.name);
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isSpecialEffectActive = false;
        GetComponent<HealthBar>().SetMaxHp(enemyHP);
    }

    private void Update()
    {
        enemyAgent.enabled = true;
    }

    private void Death() {
        killed.Invoke();
 //       Debug.Log("Enemy killed");
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
                specialEffectDurationInSec = b.GetSpecialEffectDuration();
                specialEffectDmgPerSec = b.GetSpecialEffectDmg();
                activateSpecialEffect(b.GetBulletType(), specialEffectDmgPerSec, specialEffectDurationInSec);
                break;
            case Resistance.low:
                Debug.Log("jest ok");
                enemyHP -= b.GetDamage() * 1.5f;
                isSpecialEffectActive = true;
                specialEffectDurationInSec = (int)(b.GetSpecialEffectDuration() * 1.5f);
                specialEffectDmgPerSec = (int)(b.GetSpecialEffectDmg() * 1.5f);
                Debug.Log("bullet type: " + b.GetBulletType() + "duration: " + specialEffectDurationInSec + " dmg: " + specialEffectDmgPerSec);
                StartCoroutine(activateSpecialEffect(b.GetBulletType(), specialEffectDmgPerSec, specialEffectDurationInSec));
                Debug.Log("mama");
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

    IEnumerator activateSpecialEffect(ElementType type, int dmgPerSec, int durationInSec)
    {
        Debug.Log("special effect entry");
        switch (type)
        {
            case ElementType.earth:
                break;
            case ElementType.electricity:
                Debug.Log("electricity special effect");
                break;
            case ElementType.fire:
                //podpal miniona
                StartCoroutine(GetComponent<FireSE>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case ElementType.ice:
                //zamroz miniona
                StartCoroutine(GetComponent<IceSE1>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case ElementType.wind:
                break;
            default:
                yield return new WaitForSeconds(0f);
                break;
        }
    }

    public void Slowdown(float slow)
    {
        enemyAgent.speed /= slow;
        speedManipulations *= slow;
    }

    public void Speedup(float speed)
    {
        enemyAgent.speed *= speed;
        speedManipulations /= speed;
    }
    
    public void SetNormalSpeed()
    {
        enemyAgent.speed *= speedManipulations;
        speedManipulations = 1f;
    }

    public ElementType GetElementType()
    {
        return type;
    }
}