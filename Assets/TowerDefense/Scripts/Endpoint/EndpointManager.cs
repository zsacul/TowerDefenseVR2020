using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndpointManager : GameEventListener
{
    [SerializeField]
    GameEvent gameLost;
    public UnityEvent damaged;
    public UnityEvent destroyed;
    public int health;
    private bool isGameOver;

    private void Start()
    {
        isGameOver = false;
    }

    bool GameLostCondition()
    {
        //GetComponent<HealthBar>().SetMaxHp(health);
        return this.health <= 0;
    }

    // Method called by the enemy after arriving to the endpoint
    void DamageEndpoint(int damageValue)
    {
        //Debug.Log("HEALTH: " + health + "; damageValue: " + damageValue);
        if (health >= 1)
        {
            this.health -= damageValue;
            GetComponent<HealthBar>().updateBar(health);

            if (GameLostCondition())
            {
                gameLost.Raise();
                destroyed.Invoke();
            }
        }
    }

    public override void OnEventRaised(Object data)
    {
        isGameOver = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            DamageEndpoint(1);
            other.transform.position = Vector3.down * 1000;
            other.GetComponent<EnemyHPManager>().Death();
            damaged.Invoke();
        }
        
    }
}
