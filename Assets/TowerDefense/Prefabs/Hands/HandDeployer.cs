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

    private void CallInit(GameObject target)
    {
        target.GetComponent<PropManager>().Initialize();
    }

    public void DeployNth(int Nth)
    {
        CallKill(PropList[listIterator].Instance);
        CallWakeup(PropList[Nth].Instance);
        listIterator = Nth;
    }

    public void DeployNext()
    {
        CallKill(PropList[listIterator].Instance);
        listIterator++;
        if (listIterator == PropList.Count)
            listIterator = 0;
        CallWakeup(PropList[listIterator].Instance);
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
