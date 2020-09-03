using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUIDestroy : MonoBehaviour, ITargetReceiver
{
    private Vector3 target;
    private BuildingDestroyer targetedManagerAux;

    private BuildingDestroyer[] managers;

    private BuildingDestroyer targetedManager 
    { 
        get => targetedManagerAux; 
        set 
        {
            if(value != targetedManagerAux)
            {
                //change
                if (targetedManager != null)
                {
                    targetedManagerAux.HoverOff();
                }
                targetedManagerAux = value;
                if(targetedManager != null)
                {
                    targetedManagerAux.HoverOn();
                }
            }
        }
    }

    public void UpdateTarget(Vector3 target)
    {
        this.target = target;
    }

    void Start()
    {
        InvokeRepeating("FindBuildingDestroyer", 0, 0.2f);
    }
    private void FindBuildingDestroyer()
    {
        managers = managers == null ? FindObjectsOfType<BuildingDestroyer>() : managers;
        if (managers.Length == 0) return;
        float minDistance = float.MaxValue;
        int index = 0;
        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i] == null) continue;
            float distance = Vector3.Distance(target - Vector3.up * target.y, managers[i].transform.position - Vector3.up * managers[i].transform.position.y);
            if (minDistance > distance)
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
        targetedManager = managers[index];
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton9) || Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f || Input.GetKeyDown(KeyCode.Mouse1))
            && targetedManager != null)
        {
            DestroyBuilding();
        }
    }
    public void CheckForManager()
    {
        if (FindObjectOfType<BuildingDestroyer>() == null)
        {
            gameObject.GetComponent<UINode>().active = false;
        }
    }
    public void DestroyBuilding()
    {
        targetedManager.DestroyBuilding();
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
}
