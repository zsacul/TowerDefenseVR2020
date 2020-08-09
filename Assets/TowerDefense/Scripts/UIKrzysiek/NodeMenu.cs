using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class NodeMenu : MonoBehaviour
{
    public NodePos[] childNodes;
    public UnityEvent onActivate;
    private List<UINode> childrenUI;
    public UnityEvent onDeactivate;
    public Transform head;
    private bool menuActive;
    private void Start()
    {
        childrenUI = new List<UINode>();
    }
    public void SpawnChildNodes()
    {
        foreach (NodePos n in childNodes)
        {
            Vector3 pos = transform.position + n.relativePosition.x * head.right +
                                               n.relativePosition.y * head.up +
                                               n.relativePosition.z * head.forward;
            GameObject o = Instantiate(n.node, pos, head.rotation);
            childrenUI.Add(o.GetComponent<UINode>());
            o.GetComponent<UINode>().onSelect.AddListener(ChildSelected);
            o.GetComponent<UINode>().menuHead = head;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("VRTK_Axis9_LeftTrigger") > 0.1f)
        {
            if(!menuActive)
            {
                onActivate.Invoke();
            }
            menuActive = true;
        }
        else
        {
            if (menuActive)
            {
                onDeactivate.Invoke();
                Dispose();
            }
            menuActive = false;
        }
    }
    public void Dispose()
    {
        foreach (UINode n in childrenUI)
        {
            if(n != null)
            {
                n.Dispose();
            }
        }
        childrenUI.Clear();
    }
    public void DisposeUnused(UINode keepAlive)
    {
        foreach (UINode n in childrenUI)
        {
            if (n != null && n != keepAlive)
            {
                n.Dispose();
            }
        }
        childrenUI.Clear();
        childrenUI.Add(keepAlive);
    }
    private void ChildSelected()
    {
        UINode keepAlive = childrenUI.ToArray().First<UINode>((UINode n) => n.selected);
        DisposeUnused(keepAlive);
    }
}
