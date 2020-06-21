using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakButtonHandler : MonoBehaviour
{

    [SerializeField]
    Transform ButtonPushedPosition;
    [SerializeField]
    Transform ButtonReleasedPosition;
    
    bool pushed;
    SpawnManaging.SpawnManager spawnManager;
    public UnityEvent ButtonClicked;
    
    private void Start()
    {
        pushed = false;
        spawnManager = FindObjectOfType<SpawnManaging.SpawnManager>();
    }

    private void Update()
    {
        if (transform.position.y <= ButtonPushedPosition.position.y && IsPlayerOnTower())
        {
            spawnManager.EndBreak();
            ButtonClicked.Invoke();
        }
        
        if (transform.position.y > ButtonReleasedPosition.position.y)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = ButtonReleasedPosition.position;
        }
    }


    private bool IsPlayerOnTower()
    {
        return Camera.main.transform.position.y > 4f;
    }

}
