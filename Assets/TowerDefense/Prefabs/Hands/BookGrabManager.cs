using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookGrabManager : PropManager
{
    private GameObject HandManger;
    private GameObject BookHook;
    // Start is called before the first frame update
    public override void GrabEvent(float input)
    {
        //Debug.Log($"overriden gevent {input}");
        if (input > 0.70f) // The object is grabbed, treat it as usual
            GizmoAnimation.SetFloat("GripFloat", input);
        else
        {
            BookHook.GetComponent<BookManager>().EnableMarkers();
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
        }
        /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true);

        // motivator is the book target
        BookHook = Motivator.transform.parent.gameObject;

        // bookhook has a function to spawn/despawn grab motivators
        BookHook.GetComponent<BookManager>().DisableMarkers();
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
        
    }
}
