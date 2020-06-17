using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakButtonHandler : MonoBehaviour
{

    bool pushed;
    SpawnManaging.SpawnManager spawnManager;
    public UnityEvent ButtonClicked;
 
    private void Start()
    {
        pushed = false;
        spawnManager = FindObjectOfType<SpawnManaging.SpawnManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(!pushed )
        {
            if (IsPlayerOnTower())
            {
                pushed = true;
                transform.localPosition += new Vector3(0f, -0.02f, 0f);
                spawnManager.EndBreak();
                ButtonClicked.Invoke();
            }
            else
                Debug.Log("Gracz znajduje się na glebie");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pushed = false;
        transform.localPosition -= new Vector3(0f, -0.02f, 0f);
    }

    private bool IsPlayerOnTower()
    {
        return Camera.main.transform.position.y > 4f;
    }

}
