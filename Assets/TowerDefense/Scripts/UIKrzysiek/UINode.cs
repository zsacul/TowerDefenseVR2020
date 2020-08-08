using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UINode : MonoBehaviour
{
    public bool active;
    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public UnityEvent onDispose;
    public UnityEvent onTouch;
    public UnityEvent onSpawned;
    private LineRenderer line;
    public UINode parentNode;
    public NodePos[] childNodes;
    private bool selected;
    private bool drawLine;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        onSpawned.Invoke();
    }
    private void Update()
    {
        if(parentNode != null && drawLine)
        {
            LineUpdate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!active) return;
        onTouch.Invoke();
        if(selected)
        {
            
        }
        else
        {
            Select();
        }
    }
    public void Select()
    {
        selected = true;
        drawLine = true;
        onSelect.Invoke();
        if(parentNode != null)
        {
            parentNode.Deselect();
        }
    }
    public void Deselect()
    {
        selected = false;
        onDeselect.Invoke();
    }
    public void SpawnChildNodes()
    {
        foreach(NodePos n in childNodes)
        {
            Vector3 pos = transform.position + n.relativePosition.x * transform.right +
                                               n.relativePosition.y * transform.up +
                                               n.relativePosition.z * transform.forward;
            GameObject o = Instantiate(n.node, pos, Quaternion.identity);
        }
    }
    public void LineUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, parentNode.transform.position);
    }
}
