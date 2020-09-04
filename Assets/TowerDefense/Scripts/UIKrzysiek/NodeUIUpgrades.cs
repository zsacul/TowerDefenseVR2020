using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeUIUpgrades : MonoBehaviour, ITargetReceiver
{
    public int cost;
    public int index;
    public Vector3 target;
    private UpgradeManager[] managers;
    private UpgradeManager targetedManager;
    public AudioClip buildClip;
    // Start is called before the first frame update
    void Start()
    {
        if (index == 0) return;//index == 0 means it is "Upgrade" node -> not actual element upgrade you choose later
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
            if (managers[i] == null) continue;
            float distance = Vector3.Distance(target - Vector3.up * target.y, managers[i].transform.position - Vector3.up * managers[i].transform.position.y);
            if(minDistance > distance)
            {
                index = i;
                minDistance = distance;
            }
        }
        if (minDistance > 4.0f)
        {
            targetedManager = null;
            NodeMenu.HideMarker();
            return;
        }
        if (targetedManager != null)
        {
            NodeMenu.SetMarker(targetedManager.transform.position);
        }
        else
        {
            NodeMenu.HideMarker();
        }
        targetedManager =  managers[index];
    }
    private void Update()
    {
        if (index != 0 && (Input.GetKeyDown(KeyCode.JoystickButton9) || Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f || Input.GetKeyDown(KeyCode.Mouse1))
            && targetedManager != null)
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
        AudioSource.PlayClipAtPoint(buildClip, Camera.main.transform.position);
        targetedManager.UpgradeTower(index, cost);
        NodeMenu.Dispose();
    }
    public void UpdateRayNode()
    {
        UpgradeTargeting.SetNode(this);
    }
    public void ActivateRay(bool state)
    {
        UpgradeTargeting.SetActiveRay(state);
    }
    private void OnDestroy()
    {
        NodeMenu.HideMarker();
    }

    public void UpdateTarget(Vector3 target)
    {
        this.target = target;
    }
}
