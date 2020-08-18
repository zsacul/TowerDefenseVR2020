using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagingRay : MonoBehaviour , IChargable
{
    public DamageData dmg;
    public UnityEvent pulse;
    public UnityEvent hit;
    public UnityEvent startPulsing;
    public UnityEvent endPulsing;
    public float repeatRate = 0.5f;
    public float length;
    public AnimationCurve lifeTimeOverCharge = AnimationCurve.Linear(0, 0, 1, 2);
    public AnimationCurve dmgMultOverCharge = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve sizeOverCharge = AnimationCurve.Linear(0, 0.15f, 1, 0.5f);

    LineRenderer line;
    bool pulsing;
    public ParticleSystem contactPoint;
    private float charge;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if(pulsing)
        {
            RayUpdate();
        }
    }
    public void Release()
    {
        dmg.damage *= dmgMultOverCharge.Evaluate(charge);
        dmg.specialEffectDmgPerSec *= dmgMultOverCharge.Evaluate(charge);
        length = lifeTimeOverCharge.Evaluate(charge);
        StartPulsing();
        Invoke("EndPulsing", length);
    }

    public void SetCharge(float charge)
    {
        this.charge = charge;
        transform.localScale = Vector3.one * sizeOverCharge.Evaluate(charge);
    }
    void DealDamage(EnemyHPManager hp)
    {
        hp.ApplyDamage(dmg);
    }
    EnemyHPManager DetectEnemy()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit))
        {
            ContactPointUpdate(true, hit.point, hit.normal);
            
            if(hit.collider.TryGetComponent<EnemyHPManager>(out var hp))
            {
                return hp;
            }
        }
        else
        {
            ContactPointUpdate(false, Vector3.zero, Vector3.zero);
        }
        return null;
    }
    void Pulse()
    {
        pulse.Invoke();
        EnemyHPManager hp = DetectEnemy();
        if(hp != null)
        {
            hit.Invoke();
            DealDamage(hp);
        }
    }
    void StartPulsing()
    {
        pulsing = true;
        InvokeRepeating("Pulse", 0, repeatRate);
        startPulsing.Invoke();
    }
    void EndPulsing()
    {
        pulsing = false;
        if(IsInvoking("Pulse"))
        {
            CancelInvoke("Pulse");
            endPulsing.Invoke();
        }
        line.enabled = false;
        Destroy(this);
        contactPoint.Stop();
        Destroy(GetComponent<Renderer>());
        Destroy(gameObject, 2);
    }
    void RayUpdate()
    {
        Vector3 endPoint;
        if(Physics.Raycast(transform.position, transform.forward, out var hit))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = transform.position + transform.forward * 100.0f;
        }
        line.SetPositions(new Vector3[]{ transform.position, endPoint});
    }
    void ContactPointUpdate(bool active, Vector3 position, Vector3 normal)
    {
        if (active)
        {
            contactPoint.Play();
            contactPoint.transform.position = position;
            contactPoint.transform.LookAt(position + normal);
        }
        else
        {
            contactPoint.Stop();
        }
    }
}
