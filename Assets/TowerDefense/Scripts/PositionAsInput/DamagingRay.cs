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
    LineRenderer line;
    bool pulsing;
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
        StartPulsing();
        Invoke("EndPulsing", length);
    }

    public void SetCharge(float charge)
    {
       
    }
    void DealDamage(EnemyHPManager hp)
    {
        hp.ApplyDamage(dmg);
    }
    EnemyHPManager DetectEnemy()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit))
        {
            if(hit.collider.TryGetComponent<EnemyHPManager>(out var hp))
            {
                return hp;
            }
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
        Destroy(gameObject);
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
}
