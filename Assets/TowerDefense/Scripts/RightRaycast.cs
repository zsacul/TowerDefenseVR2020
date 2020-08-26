using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RightRaycast : MonoBehaviour
{
    private bool teleportState;
    private bool buildingState;

    public UnityEvent TeleportOn;
    public UnityEvent TeleportOff;
    public UnityEvent BuildingOn;
    public UnityEvent BuildingOff;

    public static RightRaycast Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        teleportState = true;
        buildingState = false;
    }

    public void TurnTeleport(bool newState)
    {
        if (newState && !teleportState)
        {
            TeleportOn.Invoke();
            //Debug.Log("Turning on teleport Raycast");
            teleportState = true;
        }
        else if (!newState)
        {
            TeleportOff.Invoke();
            //Debug.Log("Turning off teleport raycast");
            teleportState = false;
        }
    }

    public void TurnBuilding(bool newState)
    {
        if (newState && !buildingState)
        {
            BuildingOn.Invoke();
            buildingState = true;
        }
        else if (!newState && buildingState)
        {
            BuildingOff.Invoke();
            buildingState = false;
        }
    }
}
