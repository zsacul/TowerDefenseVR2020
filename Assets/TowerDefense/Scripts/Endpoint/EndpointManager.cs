using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndpointManager : GameEventListener
{
    [SerializeField]
    GameEvent gameOver;
    public UnityEvent damaged;
    public UnityEvent destroyed;
    public int health;

    bool GameOverCondition()
    {
        //GetComponent<HealthBar>().SetMaxHp(health);
        return this.health <= 0;
    }

    // Method called by the enemy after arriving to the endpoint
    void DamageEndpoint(int damageValue)
    {
        //Debug.Log("HEALTH: " + health + "; damageValue: " + damageValue);
        if (!GameOverCondition())
        {
            this.health -= damageValue;
            GetComponent<HealthBar>().updateBar(health);
        }
        else
        {
            gameOver.Raise();
            destroyed.Invoke();
        }
    }

    public override void OnEventRaised(Object data)
    {
        Debug.Log("Game over");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            DamageEndpoint(1);
            other.transform.position = Vector3.down * 1000;
            other.GetComponent<EnemyHPManager>().ApplyDamage(1000000);
            damaged.Invoke();
        }
        
    }
}
