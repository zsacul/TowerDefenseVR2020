using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentMotivator : MonoBehaviour {
    public GameObject target1;
    public GameObject target2;
    public Transform goal;

    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Ping1();
        //agent.destination = goal.position;
    }

    public void Ping1() {
        agent.destination = target2.GetComponent<Transform>().position;
    }

    public void Ping2() {
        agent.destination =  target1.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update() {
    }
}
