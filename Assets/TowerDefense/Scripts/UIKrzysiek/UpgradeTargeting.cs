using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTargeting : MonoBehaviour
{
    private ITargetReceiver receiver;
    private bool active;
    Vector3 target;
    public LineRenderer line;
    // Start is called before the first frame update
    static UpgradeTargeting instance;
    private void Awake()
    {
        instance = this;
    }
    
    static public void SetActiveRay(bool state)
    {
        instance.active = state;
        instance.line.enabled = state;
    }
    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            UpdateRay();
            UpdateLine();
        }
        else
        {
            
        }
    }
    private void UpdateRay()
    {
        if(Physics.Raycast(transform.position + transform.forward, transform.forward, out RaycastHit info))
        {
            receiver.UpdateTarget(info.point);
            target = info.point;
        }
        else
        {
            receiver.UpdateTarget(transform.position + transform.forward * 100.0f);
            target = transform.position + transform.forward * 100.0f;
        }
    }
    private void UpdateLine()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target);
    }
    static public void SetNode(ITargetReceiver node)
    {
        instance.receiver = node;
    }
}
