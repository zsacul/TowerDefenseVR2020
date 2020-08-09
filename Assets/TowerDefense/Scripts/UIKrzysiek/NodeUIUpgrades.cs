using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeUIUpgrades : MonoBehaviour
{
    public int cost;
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
            float distance = Vector3.Distance(transform.position, managers[i].transform.position);
            if(minDistance > distance)
            {
                index = i;
                minDistance = distance;
            }
        }
        Debug.DrawRay(managers[index].transform.position, Vector3.up * 1000, Color.red);
        return managers[index];
    }
    public void CheckForManager()
    {
        if(FindUpgradeManager() == null)
        {
            gameObject.GetComponent<UINode>().Dispose();
        }
    }
    public void UpgradeTower(int index)
    {
        FindUpgradeManager().UpgradeTower(index, cost);
    }
}
