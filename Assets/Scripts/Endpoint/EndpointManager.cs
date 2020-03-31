using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndpointManager : GameEventListener
{
    [SerializeField]
    GameEvent gameOver;

    public int health;

    bool GameOverCondition()
    {
        return this.health <= 0;
    }

    // Method called by the enemy after arriving to the endpoint
    void DamageEndpoint(int damageValue)
    {
        this.health -= damageValue;
        if (GameOverCondition())
        {
            gameOver.Raise();
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
            other.GetComponent<EnemyHPManager>().ApplyDamage(1000000);
        }
        
    }
}
