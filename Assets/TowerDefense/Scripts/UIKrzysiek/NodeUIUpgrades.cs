using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeUIUpgrades : MonoBehaviour
{
    public int cost;
    public int index;
    public Vector3 target;
    private UpgradeManager[] managers;
    private UpgradeManager targetedManager;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindUpgradeManager", 0, 0.2f);
    }
    private void FindUpgradeManager()
    {
        managers = managers == null ? FindObjectsOfType<UpgradeManager>() : managers;
        if (managers.Length == 0) return;
        float minDistance = float.MaxValue;
        int index = 0;
        for(int i=0; i<managers.Length;i++)
        {
            float distance = Vector3.Distance(target - Vector3.up * target.y, managers[i].transform.position - Vector3.up * managers[i].transform.position.y);
            if(minDistance > distance)
            {
                index = i;
                minDistance = distance;
            }
            if (distance > 4)
            {
                targetedManager = null;
                return;
            }
        }
        
        Debug.DrawRay(managers[index].transform.position, Vector3.up * 1000, Color.red);
        targetedManager =  managers[index];
    }
    private void Update()
    {
        if(index != 0 && Input.GetKeyDown(KeyCode.JoystickButton9) && targetedManager != null)
        {
            UpgradeTower(index);
        }
    }
    public void CheckForManager()
    {
        if(FindObjectOfType<UpgradeManager>() == null)
        {
            gameObject.GetComponent<UINode>().active = false;
        }
    }
    public void UpgradeTower(int index)
    {
        targetedManager.UpgradeTower(index, cost);
    }
    public void UpdateRayNode()
    {
        UpgradeTargeting.SetNode(this);
    }
    public void ActivateRay(bool state)
    {
        UpgradeTargeting.SetActiveRay(state);
    }
}
