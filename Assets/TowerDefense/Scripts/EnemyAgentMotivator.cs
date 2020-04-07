using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentMotivator : MonoBehaviour {
    public Transform target1;
    public Transform goal;

    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        Ping1();
        //agent.destination = goal.position;
    }

    public void Ping1() {
        agent.destination = target1.position;
    }

    // Update is called once per frame
    void Update() {
    }
}
