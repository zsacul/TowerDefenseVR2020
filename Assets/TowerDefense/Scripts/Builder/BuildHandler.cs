using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject canBuild;
    [SerializeField]
    private GameObject cantBuild;
    private BuildManager buildManager;
    private BoxCollider thisBoxCollider;

    // Object that's being instantiated after player in Build Mode points to this chunk 
    private GameObject buildAvailUI;
    private GameObject rightController;

    // Checks if controller points to this chunk
    private bool pointedAt;
    // False if buildAvailUI is already instantiated
    private bool shouldCallHover;

    private ChunkType selectedBuilding;
    private int sBuildingCost; 

    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        thisBoxCollider = gameObject.AddComponent<BoxCollider>();
        thisBoxCollider.enabled = false;
        pointedAt = false;
        shouldCallHover = true;
        rightController = buildManager.rightController;
    }

    void Update()
    {
        // Switch on Chunk's collider if building mode is turned on. 
        if (buildManager.UpdateModeCond())
        {
            thisBoxCollider.enabled = buildManager.BuildModeOn;
        }

        if (pointedAt)
        {
            ChunkType previousBuilding = selectedBuilding;
            UpdateSelectedBuilding();
            if(previousBuilding != selectedBuilding)
            {
                HoverOff();
                HoverOn();
            }
            UpdatePointedAt();
        }
    }

    private void UpdatePointedAt()
    {
        RaycastHit hit;
        //Checking if the controller still points at this chunk 
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 10000))
        {
            pointedAt = (hit.collider.gameObject.transform.position == transform.position);
        }
        else
        {
            pointedAt = false;
        }

        //If this chunk is no longer pointed at, we call function HoverOff
        if (!pointedAt)
        {
            HoverOff();
        }
        //If this chunk is still being pointed at and player presses Enter, we should build
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Build();
            }

        }
    }

    private void UpdateSelectedBuilding()
    {
        System.Tuple<ChunkType, int> selectedBuildingInfo = buildManager.ActiveBuildingInfo;
        selectedBuilding = selectedBuildingInfo.Item1;
        sBuildingCost = selectedBuildingInfo.Item2;
    }

    private void Build()
    {
        // Checking if BuildMode is switched on
        if (buildManager.BuildModeOn)
        {
            UpdateSelectedBuilding();
            //Debug.Log("PRESSED ON TILE. SELECTED BUILDING = " + building.ToString());
            if (buildManager.Money >= sBuildingCost)
            {
                if (gameObject.GetComponent<Chunk>().ChangeType(selectedBuilding))
                {
                    buildManager.DecreaseMoney(sBuildingCost);
                    buildManager.Success();
                    //Debug.Log("SUCCESS");
                }
                else
                {
                    buildManager.Failure();
                    //Debug.Log("FAILURE");
                }
            }
        }
       // HoverOn();
    }
    public void SeeYou()
    {
        pointedAt = true;
        if (shouldCallHover)
        {
            HoverOn();
        }

    }
    public void HoverOn()
    {
        shouldCallHover = false;
        UpdateSelectedBuilding();
        if (gameObject.GetComponent<Chunk>().ValidOperation(selectedBuilding) && buildManager.Money >= sBuildingCost)
        {
            buildAvailUI = Instantiate(canBuild, transform.position, transform.rotation);
        }
        else
        {
            buildAvailUI = Instantiate(cantBuild, transform.position, transform.rotation);
        } 
    }

    public void HoverOff()
    {
        Destroy(buildAvailUI);
        shouldCallHover = true;
    }
}
