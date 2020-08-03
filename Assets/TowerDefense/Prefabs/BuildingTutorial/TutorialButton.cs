using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialButton : MonoBehaviour
{
    public UnityEvent OnStart;

    private bool pushed;
    TeleportMainNode teleportTut;

    void Start()
    {
        pushed = false;
        teleportTut = FindObjectOfType<TeleportMainNode>();
        if (teleportTut == null)
            Debug.LogError("Brak tutoriala do teleportu!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            transform.localPosition += new Vector3(0f, -0.06f, 0f);
            OnStart.Invoke();
            teleportTut.StartThisTutorial();
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
