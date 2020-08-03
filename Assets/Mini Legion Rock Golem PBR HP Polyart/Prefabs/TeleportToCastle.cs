using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToCastle : TeleportSubNode
{
    // Start is called before the first frame update
    void Start()
    {
        state = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Camera.main.transform.position.y > 3.5f)
            SetNextStep();
    }
}
