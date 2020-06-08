using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookGrabManager : PropManager
{
    private GameObject HandManger;
    private GameObject BookHook;
    public GameObject MpageHook;
    public bool grabbing;
    // Start is called before the first frame update
    public override void GrabEvent(float input)
    {
        //Debug.Log($"overriden gevent {input}");
        if (input > 0.70f) // The object is grabbed, treat it as usual
            GizmoAnimation.SetFloat("GripFloat", input);
        else
        {
            BookHook.GetComponent<BookManager>().LeaveBook();
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
            grabbing = false;
        }
        /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true);

        // motivator is the book target
        BookHook = Motivator.transform.parent.gameObject.transform.parent.gameObject;

        char MotivatorName = Motivator.name[10];

        if (MotivatorName == 'L') 
            BookHook.GetComponent<BookManager>().TurnLeft();
        if (MotivatorName == 'R')
            BookHook.GetComponent<BookManager>().TurnRight();

        // bookhook has a function to spawn/despawn grab motivators
        BookHook.GetComponent<BookManager>().DisableMarkers();

        BookHook.GetComponent<BookManager>().VRPageControll = true;

        grabbing = true;

        MpageHook = BookHook.transform.GetChild(0).gameObject;
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbing)
        {
            Vector3 Delta = transform.position - MpageHook.transform.position;
            Delta.z = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(Delta);
            Vector3 EulerRot = rotation.eulerAngles;

            Debug.Log(EulerRot.y);
            if ((int)EulerRot.y > 180)
            {
                Debug.Log("A");
                EulerRot.z = 180.0f + EulerRot.x;
            }
            else
            {
                Debug.Log("B");
                EulerRot.z = - EulerRot.x;
            }

            EulerRot.y = 0.0f;
            EulerRot.x = 0.0f;
            MpageHook.transform.localRotation = Quaternion.Euler(EulerRot);
        }
    }
}
