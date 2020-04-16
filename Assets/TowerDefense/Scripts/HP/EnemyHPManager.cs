#define DEBUG
//#undef DEBUG

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EnemyHPManager : MonoBehaviour {
    float speedManipulations;
    float specialEffectDurationInSec;
    bool isSpecialEffectActive;
    int specialEffectDmgPerSec;
    ElementType bulletType;
    NavMeshAgent enemyAgent;
    Vector3 last_velocity;

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
        
    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isSpecialEffectActive = false;
        GetComponent<HealthBar>().SetMaxHp(enemyHP);
    }

    private void Death() {
        killed.Invoke();
        Destroy(GetComponent<Collider>());
        Destroy(enemyAgent);
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
                activateSpecialEffect(b.GetSpecialEffect(), specialEffectDmgPerSec, specialEffectDurationInSec);
                break;
            case Resistance.low:
                enemyHP -= b.GetDamage() * 1.5f;
                specialEffectDurationInSec = (int)(b.GetSpecialEffectDuration() * 1.5f);
                specialEffectDmgPerSec = (int)(b.GetSpecialEffectDmg() * 1.5f);
                StartCoroutine(activateSpecialEffect(b.GetSpecialEffect(), specialEffectDmgPerSec, specialEffectDurationInSec));
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

    IEnumerator activateSpecialEffect(SpecialEffect SEtype, int dmgPerSec, float durationInSec)
    {
        switch (SEtype)
        {
            case SpecialEffect.EarthSE1:
                StartCoroutine(GetComponent<EarthSE1>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case SpecialEffect.ElectricitySE1:
                StartCoroutine(GetComponent<ElectricitySE>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case SpecialEffect.FireSE1:
                StartCoroutine(GetComponent<FireSE>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case SpecialEffect.IceSE1:
                StartCoroutine(GetComponent<IceSE1>().RunSpecialEffect(this, dmgPerSec, durationInSec));
                break;
            case SpecialEffect.WindSE1:
                //Nic sie nie dzieje
                break;
            default:
                yield return new WaitForSeconds(0f);
                break;
        }
    }

    public ElementType GetElementType()
    {
        return type;
    }
}