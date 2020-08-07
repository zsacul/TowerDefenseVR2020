using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RightHandHoldingListener : MonoBehaviour
{
    public GameObject RightHand;
    public UnityEvent GrabbedSomething;
    public UnityEvent DroppedSomething; 

    private HandDeployer rightHandDeployer;
    private int previousItemInHand;
    private void Start()
    {
        rightHandDeployer = RightHand.GetComponent<HandDeployer>();
        previousItemInHand = 0;
    }

    private void Update()
    {
        if(previousItemInHand == 0 && rightHandDeployer.listIterator != 0)
        {
            GrabbedSomething.Invoke();
        }
        else if(previousItemInHand != 0 && rightHandDeployer.listIterator == 0)
        {
            DroppedSomething.Invoke();
        }
    }
}
