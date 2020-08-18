#define DEBUG
//#undef DEBUG

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Zinnia.Tracking.CameraRig;
using TMPro;

public class EnemyHPManager : MonoBehaviour {
    bool isDead;
    float speedManipulations;
    float specialEffectDurationInSec;
    bool isSpecialEffectActive;
    int specialEffectDmgPerSec;
    ElementType bulletType;
    NavMeshAgent enemyAgent;
    Vector3 last_velocity;
    MaterialPropertyBlock MPB;

    public UnityEvent damaged;
    public UnityEvent killed;
    [Min(1f)]
    public float enemyHP = 100f;
    public int moneyDropped = 0;

    [SerializeField]
    Elements elementsInfo;
    [SerializeField]
    GameObject targetPoint;
    [SerializeField]
    ElementType type;
    [SerializeField]
    Canvas DmgText;
        
    private void Start()
    {
        isDead = false;
        enemyAgent = GetComponent<NavMeshAgent>();
        elementsInfo = GameObject.Find("GameManager").GetComponent<Elements>();
        isSpecialEffectActive = false;
        GetComponent<HealthBar>().SetMaxHp(enemyHP);
    }

    public void Death() {
        //Debug.Log("Death() called");
        isDead = true;
        killed.Invoke();
        Destroy(GetComponent<Collider>());
        Destroy(enemyAgent);
        Destroy(gameObject, 3);
    }

    public void ApplyDamage(Bullet b) {
       // Debug.Log("ApplyDamage() called");
        Resistance res = elementsInfo.GetResistence(type, b.GetBulletType());

        switch (res)
        {
            case Resistance.high:
                ApplyDamage(b.GetDamage() * 0.7f);
                break;
            case Resistance.normal:
                ApplyDamage(b.GetDamage());
                specialEffectDurationInSec = b.GetSpecialEffectDuration();
                specialEffectDmgPerSec = b.GetSpecialEffectDmg();
                activateSpecialEffect(b.GetSpecialEffect(), specialEffectDmgPerSec, specialEffectDurationInSec);
                break;
            case Resistance.low:
                ApplyDamage(b.GetDamage() * 1.5f);
                specialEffectDurationInSec = (int)(b.GetSpecialEffectDuration() * 1.5f);
                specialEffectDmgPerSec = (int)(b.GetSpecialEffectDmg() * 1.5f);
                StartCoroutine(activateSpecialEffect(b.GetSpecialEffect(), specialEffectDmgPerSec, specialEffectDurationInSec));
                break;
            default:
                break;
        };
        
        GetComponent<HealthBar>().updateBar(enemyHP);
        b.SetReadyToDestroy();

        if (enemyHP <= 0 && !isDead) {
            BuildManager.Instance.AddMoney(moneyDropped);
            Death();
        }
    }
    public void ApplyDamage(DamageData data)
    {
        Resistance res = elementsInfo.GetResistence(type, data.element);

        switch (res)
        {
            case Resistance.high:
                ApplyDamage(data.damage * 0.7f);
                break;
            case Resistance.normal:
                ApplyDamage(data.damage);
                activateSpecialEffect(data.specialEffect, (int)data.specialEffectDmgPerSec, data.specialEffectDurationInSec);
                break;
            case Resistance.low:
                ApplyDamage(data.damage * 1.5f);
                activateSpecialEffect(data.specialEffect, (int)(data.specialEffectDmgPerSec * 1.5f), data.specialEffectDurationInSec * 1.5f);
                break;
            default:
                break;
        };
        GetComponent<HealthBar>().updateBar(enemyHP);
    }
    public void ApplyDamage(float damage)
    {
        if (damage > 0)
        {
            Canvas NewDmgText = Instantiate(DmgText);
            NewDmgText.transform.parent = targetPoint.transform;
            NewDmgText.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        }
        enemyHP -= damage;

        GetComponent<HealthBar>().updateBar(enemyHP);
        if (enemyHP <= 0 && !isDead)
        {
            BuildManager.Instance.AddMoney(moneyDropped);
            Death();
        }
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