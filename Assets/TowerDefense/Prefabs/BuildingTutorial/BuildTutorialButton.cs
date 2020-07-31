using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildTutorialButton : MonoBehaviour
{
    public UnityEvent OnStart;

    private bool pushed;

    void Start()
    {
        pushed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            transform.localPosition += new Vector3(0f, -0.06f, 0f);
            OnStart.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            StartCoroutine(Exit());
        }
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(1f);
        pushed = false;
        transform.localPosition += new Vector3(0f, 0.06f, 0f);
    }
}
