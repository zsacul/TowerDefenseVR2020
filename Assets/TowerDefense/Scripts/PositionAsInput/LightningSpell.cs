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
        try
        {
            EnemyHPManager hp = FindObjectsOfType<EnemyHPManager>().ToArray().First((EnemyHPManager hpt) =>
            Vector3.Distance(hpt.transform.position, transform.position) < distance);
            createLightning(gameObject, hp.gameObject);
            return hp;
        }
        catch(Exception e)
        {
            return null;
        }       
    }
    void createLightning(GameObject start, GameObject end)
    {
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        DigitalRuby.LightningBolt.LightningBoltScript l = instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
        l.StartObject = start;
        l.EndObject = end;
        l.Duration = 0.5f;
        Debug.Log("created lightning");
        Destroy(l.gameObject, 0.5f);
    }
}
