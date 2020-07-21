using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutActivatePointer : TeleportSubNode
{
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }
    
}
