using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class UINode : MonoBehaviour
{
    private bool actives = true;
    public bool active
    {
        get => actives;
        set
        {
            actives = value;
            OnActiveChanged();
        }
    }

    private void OnActiveChanged()
    {
        GetComponent<Renderer>().material = active ? activeMaterial : blockedMaterial;
    }

    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public UnityEvent onDispose;
    public UnityEvent onTouch;
    public UnityEvent onTouchDisabled;
    public UnityEvent onSpawned;
    public Material activeMaterial;
    public Material blockedMaterial;
    [HideInInspector] public Transform menuHead;
    private LineRenderer line;
    [HideInInspector]public UINode parentNode;
    public NodePos[] childNodes;
    private List<UINode> childrenUI;
    [HideInInspector] public bool selected;
    private bool drawLine;
    private void Start()
    {
        Debug.Log("START " + name);
        childrenUI = new List<UINode>();
        line = GetComponent<LineRenderer>();
        onSpawned.Invoke();
        if (name == "Tower(Clone)")
        {
            active = BuildManager.CanBuildTower();
        }
        if(name == "Obstacle(Clone)")
        {
            active = BuildManager.CanBuildObstacle();
        }
    }
    private void Update()
    {
        if(parentNode != null && drawLine)
        {
            line.enabled = true;
            LineUpdate();
        }
        else
        {
            line.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "HandCollider") return;
        if (!active)
        {
            onTouchDisabled.Invoke();
            return;
        }
        onTouch.Invoke();

        if (selected)
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
        foreach(UINode n in childrenUI)
        {
            if(n != null)
            {
                n.Dispose();
            }
        }
        childrenUI.Clear();
        SpawnChildNodes();
        if(parentNode != null)
        {
            parentNode.Deselect();
        }
    }
    public void Deselect()
    {
        selected = false;
        onDeselect.Invoke();
        try
        {
            UINode keepAlive = childrenUI.ToArray().First<UINode>((UINode n) => n.selected);
            DisposeUnused(keepAlive);
        }
        catch(Exception e)
        {
            DisposeUnused(null);
        }
    }
    public void SpawnChildNodes()
    {
        foreach(NodePos n in childNodes)
        {
            Vector3 pos = transform.position + n.relativePosition.x * menuHead.right +
                                               n.relativePosition.y * menuHead.up +
                                               n.relativePosition.z * menuHead.forward;
            GameObject o = Instantiate(n.node, pos, menuHead.rotation);
            childrenUI.Add(o.GetComponent<UINode>());
            o.GetComponent<UINode>().parentNode = this;
            o.GetComponent<UINode>().menuHead = menuHead;
        }
    }
    public void LineUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, parentNode.transform.position);
    }
    public void Dispose()
    {
        if(selected)
        {
            onDeselect.Invoke();
        }
        onDispose.Invoke();
        foreach(UINode n in childrenUI)
        {
            n.Dispose();
        }
        Destroy(gameObject);
    }
    public void DisposeUnused(UINode keepAlive)
    {
        foreach (UINode n in childrenUI)
        {
            if(n != null && n != keepAlive)
            {
                n.Dispose();
            }
        }
        childrenUI.Clear();
        childrenUI.Add(keepAlive);
    }
    public void UsePersistantState(string stateName)
    {
        active = NodeMenu.GetPersistantState(stateName);
    }
}
