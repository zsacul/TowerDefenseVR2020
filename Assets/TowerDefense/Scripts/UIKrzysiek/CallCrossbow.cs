using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCrossbow : MonoBehaviour
{
    GameObject bow;
    // Start is called before the first frame update
    void Start()
    {
        bow = FindObjectOfType<GrababbleManager>().gameObject;
    }
    public void CallCrossBow()
    {
        if (bow != null && bow.name.Contains("Crossbow"))
        {
            bow.transform.position = transform.position;
        }
    }
}
