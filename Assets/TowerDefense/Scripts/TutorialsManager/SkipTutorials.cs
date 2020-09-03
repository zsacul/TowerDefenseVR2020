using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipTutorials : MonoBehaviour
{
    private bool pushed;
    private bool skipped;
    int counter;
    public UnityEvent OnPush;

    private void Start()
    {
        pushed = false;
        skipped = false;
        counter = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            StartCoroutine(PushButton());
        }
    }

    IEnumerator PushButton()
    {
        this.transform.position += new Vector3(0f, -0.02f, 0f);
        OnPush.Invoke();
        if (!skipped)
        {
            SkipAll();
        }
        yield return new WaitForSeconds(1.0f);
        this.transform.position += new Vector3(0f, 0.02f, 0f);
        pushed = false;
    }

    public void SkipAll()
    {
        StartCoroutine(SkipAndWait());
    }

    IEnumerator SkipAndWait()
    {
        SkipOne("TutorialFirstTeleport", "TeleportMainNode");
        yield return new WaitForSeconds(0.02f);
        SkipOne("CrossbowTutorial", "CrossbowMainNode");
        yield return new WaitForSeconds(0.02f);
        SkipOne("BuildingTutorial", "BuildTutorialManager");
        yield return new WaitForSeconds(0.02f);
        SkipOne("SpellsTutorial", "SpellsTutorialMainNode");
        yield return new WaitForSeconds(0.02f);
        skipped = true;

    }


    // Finds the tutorial given by objectName, takes its script given by scriptName and calls Skip()
    private void SkipOne(string objectName, string scriptName)
    {
        var tutorial = GameObject.Find(objectName);
        if (tutorial)
        {
            var script = tutorial.GetComponent(scriptName) as MonoBehaviour;
            if (script)
            {
                script.Invoke("Skip", 0f);
            }
            else
            {
                Debug.LogError(scriptName + " not found in " + objectName);
            }
        }
        else
        {
            Debug.LogError(objectName + " not found");
        }
    }
}
