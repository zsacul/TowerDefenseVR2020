using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour
{
    public bool pushed;
    private GameObject buttonInstance;

    private void Start()
    {
        pushed = false;
        buttonInstance = transform.parent.gameObject;
    }

    public void ResetButton()
    {
        transform.localPosition += new Vector3(0f, 0.1f, 0f);
        pushed = false;
    }

    public virtual void OnButtonPush()
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            OnButtonPush();
            transform.localPosition += new Vector3(0f, -0.1f, 0f);
        }
    }

}
