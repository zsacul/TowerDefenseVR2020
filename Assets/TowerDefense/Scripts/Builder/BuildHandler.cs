using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildHandler : GameEventListener
{
    [SerializeField]
    private GameObject canBuildTower;
    [SerializeField]
    private GameObject cantBuildTower;
    [SerializeField]
    private GameObject canBuildObstacle;
    [SerializeField]
    private GameObject cantBuildObstacle;

    public UnityEvent TowerBuilt;
    public UnityEvent ObstacleBuilt;

    private BuildManager buildManager;
    private BoxCollider thisBoxCollider;
    private bool buildButtonPressed;

    // Object that's being instantiated after player in Build Mode points to this chunk 
    private GameObject showedBuilding;
    private GameObject rightController;

    // Checks if controller points to this chunk
    private bool pointedAt;
    // False if buildAvailUI is already instantiated
    private bool shouldCallHover;

    private ChunkType selectedBuilding;
    private int selectedBuildingCost;

    void Start()
    {
        buildButtonPressed = false;
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        thisBoxCollider = gameObject.AddComponent<BoxCollider>();
        thisBoxCollider.size = new Vector3(2, 0.1326768f, 2);
        thisBoxCollider.enabled = true;
        pointedAt = false;
        shouldCallHover = true;
        rightController = buildManager.rightController;
    }

    // When selected building changes to ChunkType.None, we should call UpdateSelectedBuilding and HoverOff 
    // in order to destroy currently displayed UI of selected building
    public override void OnEventRaised(Object data)
    {
        UpdateSelectedBuilding();
        HoverOff();
    }

    void Update()
    {
        if (pointedAt)
        {
            ChunkType previousBuilding = selectedBuilding;
            UpdateSelectedBuilding();
            if (previousBuilding != selectedBuilding)
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
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 10000, ~(1 << 15)))
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
        //If this chunk is still being pointed at and player presses C, we should build
        else
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton9))
            {
                Build();
                HoverOff();
            }

        }
    }

    private void UpdateSelectedBuilding()
    {
        System.Tuple<ChunkType, int> selectedBuildingInfo = buildManager.ActiveBuildingInfo;
        selectedBuilding = selectedBuildingInfo.Item1;
        selectedBuildingCost = selectedBuildingInfo.Item2;
    }

    private void Build()
    {
        // Checking if BuildMode is switched on
        if (buildManager.BuildModeOn)
        {
            UpdateSelectedBuilding();
            //Debug.Log("PRESSED ON TILE. SELECTED BUILDING = " + building.ToString());
            if (buildManager.Money >= selectedBuildingCost)
            {
                if (gameObject.GetComponent<Chunk>().ChangeType(selectedBuilding))
                {
                    buildManager.DecreaseMoney(selectedBuildingCost);
                    buildManager.Success();
                    if (selectedBuilding == ChunkType.tower)
                    {
                        TowerBuilt.Invoke();
                    }
                    else if(selectedBuilding == ChunkType.playerObstacle)
                    {
                        ObstacleBuilt.Invoke();
                    }
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
        if (gameObject.GetComponent<Chunk>().ValidOperation(selectedBuilding, false) && buildManager.Money >= selectedBuildingCost)
        {
            if (selectedBuilding == ChunkType.tower)
                showedBuilding = Instantiate(canBuildTower, transform.position, transform.rotation);
            else
                showedBuilding = Instantiate(canBuildObstacle, transform.position, transform.rotation);

        }
        else
        {
            if (selectedBuilding == ChunkType.tower)
                showedBuilding = Instantiate(cantBuildTower, transform.position, transform.rotation);
            else
                showedBuilding = Instantiate(cantBuildObstacle, transform.position, transform.rotation);
        }
    }

    public void HoverOff()
    {
        if (showedBuilding != null)
        {
            Destroy(showedBuilding);
        }
        shouldCallHover = true;
    }
}
