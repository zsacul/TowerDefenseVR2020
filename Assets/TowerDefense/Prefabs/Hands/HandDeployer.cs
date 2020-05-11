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
    [SerializeField]
    private List<GOArray> PropList;
    private GameObject CurrentlyDeployed;
    // Start is called before the first frame update
    public string HandDeployerName;
    public int listIterator;

    private void CallKill(GameObject target)
    {
        target.GetComponent<PropManager>().Remove();
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
        //Debug.Log($"Call To deploy nth {Nth}");
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

    public void TriggerHook(float input)
    {
        PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(input);
    }

    public void GrabHook(float input)
    {
        PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(input);
        if (listIterator == 0 && input > 0.7f) // hand is empty and we are grabbing shit.
        {
            Collider[] LocatedNearby = Physics.OverlapSphere(transform.position, 1.0f);
            int i = 0;
            while (i < LocatedNearby.Length)
            {
                if (LocatedNearby[i].gameObject.layer == 20)
                {
                    Debug.Log($"{LocatedNearby[i].name} :-propid-> {LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID}");
                    DeployNth(LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID, LocatedNearby[i].gameObject);
                    break;
                }
            i++;
            }
        }
    }

    public void PointyHook(bool input)
    {
        PropList[listIterator].Instance.GetComponent<PropManager>().ThumbEvent(input);
    }

    void Start()
    {
        foreach(GOArray Prop in PropList)
        {
            Prop.Instance = Instantiate(Prop.Prefab, transform); // create a new prop.
            CallInit(Prop.Instance); // initialize the object
            CallKill(Prop.Instance); // remove the initialized object
        }

        // finally call Wakeup on the zero'th object
        CallWakeup(PropList[0].Instance);
        listIterator = 0;
    }

// Update is called once per frame
void Update()
    {
        
    }
}
