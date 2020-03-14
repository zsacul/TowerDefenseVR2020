using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerEnemiesCollisionList : MonoBehaviour
{
    List<GameObject> currentCollisions = new List<GameObject>();

    private void FixedUpdate()
    {
        foreach (GameObject a in currentCollisions)
        {
            if (a == null)
                currentCollisions.Remove(a);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
       if (collision.tag == "Enemy")
        {
            currentCollisions.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        currentCollisions.Remove(collision.gameObject);
    }

    public List<GameObject> getCollidersList()
    {
        return currentCollisions;
    }

    public bool isOnCollisionList(GameObject go)
    {
        return currentCollisions.Contains(go);
    }
}
