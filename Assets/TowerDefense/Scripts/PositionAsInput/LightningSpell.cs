using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

public class LightningSpell : MonoBehaviour, IChargable
{
    public float distance;
    public DamageData dmg;
    public UnityEvent hit;
    public UnityEvent fail;
    [ContextMenuItem("Release", "Release")]
    public DigitalRuby.LightningBolt.LightningBoltScript l;
    private float charge;
    
    public void Release()
    {
        Destroy(gameObject, 1.0f);
        EnemyHPManager hp = DetectEnemy();
        if (hp != null)
        {
            hit.Invoke();
            DealDamage(hp);
            CreateLightning(gameObject, hp.gameObject);
        }
        else
        {
            fail.Invoke();
        }
    }

    public void SetCharge(float charge)
    {
        this.charge = charge;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DealDamage(EnemyHPManager hp)
    {
        dmg.damage *= charge;
        hp.ApplyDamage(dmg);
    }
    EnemyHPManager DetectEnemy()
    {
        EnemyHPManager[] hp = FindObjectsOfType<EnemyHPManager>();
        if (hp.Length == 0) return null;
        int nearestIndex = 0;
        float distanceMin = int.MaxValue;
        for(int i = 0; i < hp.Length; i++)
        {
            float dist = Vector3.Distance(hp[i].transform.position, transform.position);
            if (distanceMin > dist)
            {
                distanceMin = dist;
                nearestIndex = i;
            }
        }
        return hp[nearestIndex];
             
    }
    void CreateLightning(GameObject start, GameObject end)
    {
        l.StartPosition = start.transform.position;
        l.EndPosition = end.transform.position + Vector3.up * 0.5f;
        Destroy(l.gameObject, 1f);
        l.Trigger();
    }
}
