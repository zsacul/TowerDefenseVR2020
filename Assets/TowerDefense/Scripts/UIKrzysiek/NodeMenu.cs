using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeMenu : MonoBehaviour
{
    public NodePos[] childNodes;
    public UnityEvent onActivate;
    private List<UINode> childrenUI;
    public UnityEvent onDeactivate;
    private bool menuActive;
    private void Start()
    {
        childrenUI = new List<UINode>();
    }
    public void SpawnChildNodes()
    {
        foreach (NodePos n in childNodes)
        {
            Vector3 pos = transform.position + n.relativePosition.x * transform.right +
                                               n.relativePosition.y * transform.up +
                                               n.relativePosition.z * transform.forward;
            GameObject o = Instantiate(n.node, pos, Quaternion.identity);
            childrenUI.Add(o.GetComponent<UINode>());
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
            n.Dispose();
        }
        childrenUI.Clear();
    }
}
