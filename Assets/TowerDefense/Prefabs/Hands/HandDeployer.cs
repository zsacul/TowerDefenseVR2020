using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GOArray
{
    public GameObject Prefab;
    public GameObject Instance;
    public string description;
    public bool Ldep, Rdep;
    public bool IntentionalGrab;
    public float IGrabDistance;
    public bool PolledGrab;
    public float PolledGrabDistance;
}

public class HandDeployer : MonoBehaviour
{
    private bool critical_enforcer = false;
    public bool retarded_controlls = true;
    [SerializeField]
    public List<GOArray> PropList;
    [SerializeField]
    GameObject grabPoint;
    private GameObject CurrentlyDeployed;
    // Start is called before the first frame update
    public string HandDeployerName;
    public int listIterator;

    public UnityEvent WeaponChange;
    public GameObject RightInteractor;

    public bool isRight;
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
        WeaponChange.Invoke();
    }

    public void DeployNth(int Nth)
    {
        Debug.Log($"Call To deploy nth {Nth}");
        CallKill(PropList[listIterator].Instance);
        CallWakeup(PropList[Nth].Instance);
        listIterator = Nth;
        WeaponChange.Invoke();
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

    public bool HandGrab(float input, bool triggered)
    {
        if (listIterator == 0) // hand is empty and we are grabbing shit.
        {
            Collider[] LocatedNearby = Physics.OverlapSphere(grabPoint.transform.position, 1.0f);
            int i = 0;
            GameObject chosen = new GameObject();
            float mindist = 100.0f;
            while (i < LocatedNearby.Length)
            {
                if (LocatedNearby[i].gameObject.tag == "Grababble")
                {
                    Debug.Log(LocatedNearby[i].gameObject.name);
                    if ((PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].Ldep && !isRight) ||
                       (PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].Rdep && isRight))
                    {
                        float grabbable_distance = Vector3.Distance(LocatedNearby[i].transform.position, grabPoint.transform.position);
                        if ((
                            triggered == true &&
                            PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].IntentionalGrab &&
                            PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].IGrabDistance < grabbable_distance
                            ) || (
                            triggered == false &&
                            PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].PolledGrab &&
                            PropList[LocatedNearby[i].gameObject.GetComponent<GrababbleManager>().PropID].PolledGrabDistance < grabbable_distance
                            )) {
                                if (mindist >= grabbable_distance)
                                {
                                if (LocatedNearby[i].gameObject.name == "DummyGrababble")
                                {
                                    if (grabbable_distance > 0.1f)
                                        break;
                                    Debug.Log("Cieciwa w łape");
                                }
                                    mindist = grabbable_distance;
                                    chosen = LocatedNearby[i].gameObject;
                                }
                            }
                    }
                }
                i++;
            }

            if (mindist < 90.0f)
            {
                Debug.Log($"{chosen.name} :-propid-> {chosen.GetComponent<GrababbleManager>().PropID}");
                DeployNth(chosen.GetComponent<GrababbleManager>().PropID, chosen);
                return true;
            }
        }

        return false;
    }

    public int stateFlipFlop;
    public float release_timeout;
    public void HisterisisGrab(float input, bool intent)
    {
        if (retarded_controlls)
        {
            if (intent == false)
            {
                if (HandGrab(1.0f, intent))
                {
                    stateFlipFlop = 2;
                    release_timeout = 1.0f;
                }
                else
                    stateFlipFlop = 0;
            }
            else
            {
                // Histerisis controll
                if (input > 0.6f && stateFlipFlop == 0)
                {
                    Debug.Log("HAND GRAB");
                    if (!critical_enforcer)
                        if (!HandGrab(1.0f, intent))
                        {
                            Debug.Log("SKIPPIN");
                            stateFlipFlop = 2;
                        }

                    PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(0.99f);
                    PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(0.99f);
                    stateFlipFlop = 1;
                }

                if (input < 0.3f && stateFlipFlop == 1)
                {
                    Debug.Log("HAND GRABRELEASE");
                    stateFlipFlop = 2;
                }

                if (input > 0.6f && stateFlipFlop == 2)
                {
                    Debug.Log("HAND RELEASEBEGIN");
                    stateFlipFlop = 3;
                }

                if (input < 0.3f && stateFlipFlop == 3)
                {
                    Debug.Log("HAND OPEN");
                    if (listIterator != 0)
                        release_timeout = 1.0f;
                    PropList[listIterator].Instance.GetComponent<PropManager>().RetardedChangeGrabState();
                    PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(0.0f);
                    PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(0.0f);
                    stateFlipFlop = 0;
                }
            }
        }
    }
    public void TriggerHook(float input)
    {
        PropList[listIterator].Instance.GetComponent<PropManager>().PointEvent(input);
    }

    public void GrabHook(float input)
    {
        if (!retarded_controlls)
        {
            PropList[listIterator].Instance.GetComponent<PropManager>().GrabEvent(input);
            HandGrab(input, true);
            
        } 
        else
        {
            if(release_timeout < 0.1f)
                HisterisisGrab(input, true);
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
                Cprop.Ldep = Prop.Ldep;
                Cprop.Rdep = Prop.Rdep;

                Cprop.PolledGrabDistance = Prop.PolledGrabDistance;
                Cprop.PolledGrab = Prop.PolledGrab;

                Cprop.IntentionalGrab = Prop.IntentionalGrab;
                Cprop.IGrabDistance = Prop.IGrabDistance;
                PropList.Add(Cprop);
            }

            isRight = false;
        } else { isRight = true;  }

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
    int fid = 0;
    void Update()
    {
        speed = (lastPosition - transform.position) * -100.0f;
        lastPosition = transform.position;

        fid += 0;
        release_timeout = Math.Max(0.0f, release_timeout - Time.deltaTime);
        if(fid > 5 && release_timeout < 0.1f && listIterator == 0)
        {
            fid = 0;
            critical_enforcer = true;
            Debug.Log("Nearby object catcher");
            HisterisisGrab(0.99f, false);
            critical_enforcer = false;
        }
    }
}
