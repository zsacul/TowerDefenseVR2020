using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private bool pushed;

    private void Start()
    {
        pushed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HandCollider" && !pushed)
        {
            pushed = true;
            Debug.Log("Before calling Upgrade()");
            StartCoroutine(Upgrade());
            Debug.Log("After calling Upgrade()");
            transform.position += new Vector3(0f, 0f, 0.05f);
        }
    }

    IEnumerator Upgrade()
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log(transform.parent.transform.parent.ToString());
        transform.parent.transform.parent.GetComponent<TowerUpgrade>().Upgrade();
    }
}
