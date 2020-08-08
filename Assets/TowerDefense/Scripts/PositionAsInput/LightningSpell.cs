﻿using System.Collections;
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
    public GameObject lightning;

    public void Release()
    {
        Destroy(gameObject, 1.0f);
        EnemyHPManager hp = DetectEnemy();
        if (hp != null)
        {
            hit.Invoke();
            DealDamage(hp);
        }
        else
        {
            fail.Invoke();
        }
    }

    public void SetCharge(float charge)
    {
        
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
    void createLightning(GameObject start, GameObject end)
    {
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        DigitalRuby.LightningBolt.LightningBoltScript l = instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
        l.StartObject = start;
        l.EndObject = end;
        l.Duration = 1f;
        Debug.Log("created lightning");
        Destroy(l.gameObject, 1f);
    }
}
