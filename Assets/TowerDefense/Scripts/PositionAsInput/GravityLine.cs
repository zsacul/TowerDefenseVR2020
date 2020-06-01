using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLine : MonoBehaviour
{
    public int segments;
    public float timeStep;
    public float gravity;
    public float startSpeed;
    public float drag;
    LineRenderer line;
    
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = transform.forward * startSpeed;
        Vector3 currentPos = transform.position;
        for(int i=0; i < line.positionCount; i++)
        {
            line.SetPosition(i, currentPos);
            currentPos += speed * timeStep;
            speed += gravity * Vector3.up * timeStep;
        }
    }
}
