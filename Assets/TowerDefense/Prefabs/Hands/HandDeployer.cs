using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOArray
{
    public GameObject Prefab;
    public GameObject Instance;
    public string description;
    public bool deployable;
}

public class HandDeployer : MonoBehaviour
{
    public bool retarded_controlls = true;
    [SerializeField]
    private List<GOArray> PropList;
    private GameObject CurrentlyDeployed;
    // Start is called before the first frame update
    public string HandDeployerName;
    public int listIterator;

    public GameObject RightInteractor;

    private Vector3 speed; // I AM SPEED
    private Vector3 lastPosition;

    private void CallKill(GameObject target)
    {
        target.GetComponent<PropManager>().Remove(speed);
    }

    private void CallWakeup(GameObject target)
    {
        target.GetComponent<PropManager>().Respawn();
    }

    private void CallWakeup(GameObject target, GameObject Motivator)
    {
        target.GetComponent<PropManager>().Respawn(Motivator);
    }


    private void CallInit(GameObject target)
    {
        target.GetComponent<PropManager>().Initialize();
    }

    public void DeployNth(int Nth, GameObject Motivator)
    {
        //Debug.Log($"Call To deploy nth {Nth}");
        CallKill(PropList[listIterator].Instance);
        CallWakeup(PropList[Nth].Instance, Motivator);
        listIterator = Nth;
    }

    public void DeployNth(int Nth)
    {
        Debug.Log($"Call To deploy nth {Nth}");
        CallKill(PropList[listIterator].Instance);
        CallWakeup(PropList[Nth].Instance);
        listIterator = Nth;
    }

    public void DeployNext()
    {
        //Debug.Log($"Call To deploy next {listIterator}");
        CallKill(PropList[listIterator].Instance);
        listIterator++;
        if (listIterator == PropList.Count)
            listIterator = 0;
        CallWakeup(PropList[listIterator].Instance);
    }

    private int stateFlipFlop;
    public void TriggerHook(float input)
    {
        if (retarded_controlls)
        {
            // Histerisis controll
            if (input > 0.6f)
            {
                PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(0.99f);
                PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(0.99f);
                if (stateFlipFlop == 0)
                {
                    if (listIterator != 0)
                    {
                        PropList[listIterator].Instance.GetComponent<PropManager>().RetardedChangeGrabState();
                    }
                    else
                    {
                        HandGrab(1.0f);
                    }
                }

                stateFlipFlop = 1;
                
            }

            if (input < 0.3f)
            {
                PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(0.0f);
                PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(0.0f);
                stateFlipFlop = 0;
            }
        }
    }

    public void GrabHook(float input)
    {
        if (!retarded_controlls)
        {
            PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(input);
            HandGrab(input);
        }
    }

    public void HandGrab(float input)
    {
        if (listIterator == 0 && input > 0.7f) // hand is empty and we are grabbing shit.
        {
            Collider[] LocatedNearby = Physics.OverlapSphere(transform.position, 1.0f);
            int i = 0;
            GameObject chosen = new GameObject();
            float mindist = 100.0f;
            while (i < LocatedNearby.Length)
            {
                if (LocatedNearby[i].gameObject.tag == "Grababble")
                {
                    if (mindist >= Vector3.Distance(LocatedNearby[i].transform.position, transform.position))
                    {
                        mindist = Vector3.Distance(LocatedNearby[i].transform.position, transform.position);
                        chosen = LocatedNearby[i].gameObject;
                    }
                }
                i++;
            }

            if (mindist < 90.0f)
            {
                Debug.Log($"{chosen.name} :-propid-> {chosen.GetComponent<GrababbleManager>().PropID}");
                DeployNth(chosen.GetComponent<GrababbleManager>().PropID, chosen);
            }
        }
    }

    public void PointyHook(bool input)
    {
        PropList[listIterator].Instance.GetComponent<PropManager>().ThumbEvent(input);
    }

    void Start()
    {
        if (transform.name == "LeftControllerAlias") // we are the follower propmanager.
        {
            PropList = new List<GOArray>(); // yeet everything

            /* steal all the props from the other interactor */
            foreach(GOArray Prop in RightInteractor.GetComponent<HandDeployer>().PropList)
            {
                GOArray Cprop = new GOArray();
                Cprop.Prefab = Prop.Prefab;
                PropList.Add(Cprop);
            }
        }

        foreach(GOArray Prop in PropList)
        {
            Prop.Instance = Instantiate(Prop.Prefab, transform); // create a new prop.
            if (transform.name == "LeftControllerAlias")
            {
                Prop.Instance.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f);
            }

            CallInit(Prop.Instance); // initialize the object
            Prop.Instance.GetComponent<PropManager>().retarded_controlls = retarded_controlls;
            CallKill(Prop.Instance); // remove the initialized object
        }

        // finally call Wakeup on the zero'th object
        CallWakeup(PropList[0].Instance);
        listIterator = 0;
    }

// Update is called once per frame
    void Update()
    {
        speed = (lastPosition - transform.position) * -100.0f;
        lastPosition = transform.position;
    }
}
