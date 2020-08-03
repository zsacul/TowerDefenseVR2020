using SpawnManaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverListener : GameEventListener
{
    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = gameObject.GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("SpawnManager not found");
        }
    }

    public override void OnEventRaised(Object data)
    {
        spawnManager.GameOver();
    }
}
