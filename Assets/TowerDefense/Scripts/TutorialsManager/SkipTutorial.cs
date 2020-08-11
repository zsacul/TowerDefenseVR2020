using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    private bool pushed;

    private void Start()
    {
        pushed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            GameObject.Find("CrossbowTutorial").GetComponent<CrossbowMainNode>().SetTutorialDone();
            GameObject.Find("TutorialFirstTeleport").GetComponent<TeleportMainNode>().SetTutorialDone();
            GameObject.Find("BuildingTutorial").GetComponent<BuildTutorialManager>().EndTutorial();
            transform.localPosition += new Vector3(0f, -0.1f, 0f);
        }
    }

}
