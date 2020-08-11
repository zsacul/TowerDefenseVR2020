using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using VRTK.Prefabs.Interactions.Interactors.ComponentTags;

public class GStringManager : PropManager
{
    GameObject grabPoint;
    public UnityEvent CrossbowLoaded;

    private GameObject HandManger;
    private GameObject CrossBow;
    private GameObject String;
    private GameObject TargetBall;

    private GameObject LockTarget;

    private bool grabbing;
    private Vector3 OriginalStringPosition;
    public override void GrabEvent(float input)
    {
        //Debug.Log($"overriden gevent {input}");
        if (input > 0.70f) // The object is grabbed, treat it as usual
            GizmoAnimation.SetFloat("GripFloat", input);
        else
        {
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
            String.transform.position = TargetBall.transform.position;
        }
        /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
        TargetBall = Motivator; //the dummy target ball.
        CrossBow = Motivator.transform.parent.gameObject; // crossbow in hand
        
        GameObject CrossBowStripped = CrossBow.transform.GetChild(3).gameObject;
        LockTarget = CrossBow.transform.GetChild(4).gameObject;

        GameObject Model = CrossBowStripped.transform.GetChild(0).gameObject;
        GameObject Retarded = Model.transform.GetChild(0).gameObject;
        String = Retarded.transform.GetChild(8).gameObject;

        OriginalStringPosition = String.transform.position;
        grabbing = true;
        CrossBow.GetComponent<CrossbowManager>().control = false;
    }


    public override void Remove(Vector3 DebreeVelocity)
    {
        transform.gameObject.SetActive(false);
        grabbing = false;
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
        grabPoint = HandManger.GetComponent<HandDeployer>().grabPoint;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbing)
            String.transform.position = grabPoint.transform.position;
        if (String == null || LockTarget == null)
            return;
        if(Vector3.Distance(String.transform.position, LockTarget.transform.position) < 0.2)
        {
            CrossBow.GetComponent<CrossbowManager>().LoadedCrossbowArrow();
            CrossBow.GetComponent<CrossbowManager>().loaded = true; // the crossbow is loaded
            CrossBow.GetComponent<CrossbowManager>().control = true;
            TargetBall.SetActive(false);
            HandManger.GetComponent<HandDeployer>().DeployNth(0); // call that we have loaded the crossbow
            HandManger.GetComponent<HandDeployer>().stateFlipFlop = 0;
        }
    }
}
