using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAgentBounceback : MonoBehaviour
{
    public GameObject agent;
    public int whereping;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 AgentPosition = agent.GetComponent<Transform>().position;
        float dist = Vector3.Distance(transform.position, AgentPosition);
        if (dist < 3.0) {
            if(whereping == 1) {
                agent.GetComponent<EnemyAgentMotivator>().Ping1();
            }

            if(whereping == 2) {
                agent.GetComponent<EnemyAgentMotivator>().Ping1();
            }
        }
    }
}
