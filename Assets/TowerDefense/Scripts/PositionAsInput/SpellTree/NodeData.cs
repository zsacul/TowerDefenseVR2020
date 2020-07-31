using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeData : MonoBehaviour
{
    public Vector2Int nodePos;
    public Wand wand;
    public UnityEvent onActivated;
    private bool used;
    private void OnCollisionEnter(Collision collision)
    {
        if(!used && !collision.gameObject.CompareTag("Bullet"))
        {
            wand.CastUpdate(nodePos);
            onActivated.Invoke();
            used = true;
        }
    }
}
