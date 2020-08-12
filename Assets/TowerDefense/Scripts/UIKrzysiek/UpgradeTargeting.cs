using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTargeting : MonoBehaviour
{
    private NodeUIUpgrades upgrade;
    private bool active;
    Vector3 target;
    public LineRenderer line;
    // Start is called before the first frame update
    static UpgradeTargeting instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        upgrade = GetComponent<NodeUIUpgrades>();
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
            upgrade.target = info.point;
            target = info.point;
        }
        else
        {
            upgrade.target = transform.position + transform.forward * 100.0f;
            target = transform.position + transform.forward * 100.0f;
        }
    }
    private void UpdateLine()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target);
    }
    static public void SetNode(NodeUIUpgrades node)
    {
        instance.upgrade = node;
    }
}
