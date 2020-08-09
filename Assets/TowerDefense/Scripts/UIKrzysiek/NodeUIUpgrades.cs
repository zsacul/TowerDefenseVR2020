using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeUIUpgrades : MonoBehaviour
{
    public int cost;
    public int index;
    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private UpgradeManager FindUpgradeManager()
    {
        UpgradeManager[] managers = FindObjectsOfType<UpgradeManager>();
        if (managers.Length == 0) return null;
        float minDistance = float.MaxValue;
        int index = 0;
        for(int i=0; i<managers.Length;i++)
        {
            float distance = Vector3.Distance(target, managers[i].transform.position);
            if(minDistance > distance)
            {
                index = i;
                minDistance = distance;
            }
        }
        Debug.DrawRay(managers[index].transform.position, Vector3.up * 1000, Color.red);
        return managers[index];
    }
    private void Update()
    {
        if(index != 0 && Input.GetKeyDown(KeyCode.JoystickButton9))
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
        FindUpgradeManager().UpgradeTower(index, cost);
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
