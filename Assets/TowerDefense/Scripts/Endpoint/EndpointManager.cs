using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndpointManager : GameEventListener
{
    [SerializeField]
    GameEvent gameOver;

    public int health;
    void Start()
    {
    }

    void Update()
    {
    }

    bool GameOverCondition()
    {
        return this.health <= 0;
    }

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
}
