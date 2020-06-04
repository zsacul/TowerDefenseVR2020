using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public GameObject Lmarker, Rmarker;
    public void EnableMarkers()
    {
        Lmarker.SetActive(true);
        Rmarker.SetActive(true);
    }

    public void DisableMarkers()
    {
        Lmarker.SetActive(false);
        Rmarker.SetActive(false);
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
