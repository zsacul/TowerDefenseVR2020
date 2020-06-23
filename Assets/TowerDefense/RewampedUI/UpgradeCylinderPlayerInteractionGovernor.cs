using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCylinderPlayerInteractionGovernor : MonoBehaviour
{
    public GameObject player;
    public GameObject ConnectedButton;
    private Vector3 StartPosition;
    public float distanceScaleFactor = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
        player = GameObject.FindWithTag("Player");
    }

    public void Enter()
    {
        ConnectedButton.SetActive(false);
    }

    public void Exit()
    {
        ConnectedButton.SetActive(true);
        ConnectedButton.transform.GetChild(0).gameObject.GetComponent<SpecialisedUpgradeButton>().ResetButton();
        /* hehe umiem programować */
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 P2Srelative = player.transform.position - StartPosition;
        P2Srelative.y = 0.0f; // ignore vertical
        float P2SD = P2Srelative.magnitude;
        if(P2SD > 3.0f)
        {
            transform.position = new Vector3(100.0f, 100.0f, 100.0f);
        }
        else
        {
            float scalefactor = 0.0f;
            if (P2SD < 0.2f)
                scalefactor = 0.2f + (0.2f - P2SD);
            else if (P2SD < distanceScaleFactor)
                scalefactor = P2SD;
            else
                scalefactor = distanceScaleFactor; // lmao image using min()

            P2Srelative = Vector3.Normalize(P2Srelative) * scalefactor; // so that we generate a relative vector with length distanceScaleFactor
            transform.position = StartPosition - P2Srelative; // akchually we want to be on the other side
        }
    }
}
