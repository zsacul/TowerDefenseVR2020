﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInHandManager : PropManager
{
    public GameObject Debree;
    private GameObject HandManger;
    public override void GrabEvent(float input)
    {
        if (!retarded_controlls)
        {
            //Debug.Log($"overriden gevent {input}");
            if (input > 0.70f) // The object is grabbed, treat it as usual
                GizmoAnimation.SetFloat("GripFloat", input);
            else
                HandManger.GetComponent<HandDeployer>().DeployNth(0);
            /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
        } 
        else
        {
            GizmoAnimation.SetFloat("GripFloat", 0.99f);
            GizmoAnimation.SetFloat("PointFloat", 0.99f);
        }
    }

    public override void RetardedChangeGrabState()
    {
        if (retarded_controlls)
        {
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
        }
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
        Destroy(Motivator);

        GizmoAnimation.SetFloat("GripFloat", 0.99f);
        GizmoAnimation.SetFloat("PointFloat", 0.99f);
    }

    public override void Remove(Vector3 DebreeVelocity)
    {
        transform.gameObject.SetActive(false);
        GameObject dispatched = Instantiate(Debree);
        dispatched.GetComponent<Transform>().position = transform.position;
        dispatched.GetComponent<Transform>().rotation = transform.rotation;
        dispatched.GetComponent<Rigidbody>().velocity = DebreeVelocity;
        dispatched.SetActive(true);
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
        GizmoAnimation.SetFloat("GripFloat", 0.99f);
        GizmoAnimation.SetFloat("PointFloat", 0.99f);
    }

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
