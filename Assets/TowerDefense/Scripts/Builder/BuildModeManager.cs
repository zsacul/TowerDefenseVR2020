using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeManager : MonoBehaviour
{
    [SerializeField]
    private int money;
    [SerializeField]
    private int towerCost;
    [SerializeField]
    private int playerObstacleCost;
    [SerializeField]
    private GameObject buildingsPanel;

    private ChunkType selectedBuilding;
    private bool buildModeOn;
    //All three texts are children of buildingsPanel. They are displayed if buildModeOn is set to true. 
    private Text moneyText;
    private Text playerObstacleText;
    private Text turretText;

    void Start()
    {
        buildModeOn = false;
        buildingsPanel.SetActive(buildModeOn);
        moneyText = buildingsPanel.transform.Find("moneyText").gameObject.GetComponent<Text>();
        playerObstacleText = buildingsPanel.transform.Find("obstacleText").gameObject.GetComponent<Text>();
        turretText = buildingsPanel.transform.Find("turretText").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (buildModeOn)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                selectedBuilding = ChunkType.tower;
                SetTextColours(Color.red, Color.black);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                selectedBuilding = ChunkType.playerObstacle;
                SetTextColours(Color.black, Color.red);
                
            }
            moneyText.text = "Money: $" + money.ToString();

        }

        // Turning on/off building mode
        if (UpdateModeCond())
        {
            buildModeOn = !buildModeOn;
            // buildingsPanel should be switched off if player is not in building mode
            selectedBuilding = ChunkType.none;
            buildingsPanel.SetActive(buildModeOn);
            if (buildModeOn)
            {
                SetTextColours(Color.black, Color.black);
            }
        }
    }

    //Sets colours of turretText and playerObstacleText
    void SetTextColours(Color fortower, Color forObstacle)
    {
        turretText.color = fortower;
        playerObstacleText.color = forObstacle;
    }

    // Currently BuildingMode is switched on/off after hitting Tab
    private bool UpdateModeCond()
    {
        return Input.GetKeyDown(KeyCode.Tab);
    }

    public bool BuildModeOn {
        get {
            return buildModeOn;
        }
    }

    public int Money {
        get {
            return money;
        }
    }

    ///<summary>
    ///Returns selected building as ChunkType and its cost as int
    ///</summary>
    public System.Tuple<ChunkType, int> ActiveBuildingInfo {
        get {
            int cost = -1;
            if(selectedBuilding == ChunkType.tower)
            {
                cost = towerCost;
            }
            else if(selectedBuilding == ChunkType.playerObstacle)
            {
                cost = playerObstacleCost;
            }
            return new System.Tuple<ChunkType, int>(selectedBuilding, cost);
        }
    }

    public void DecreaseMoney(int decVal)
    {
        money -= decVal;
    }

    public void AddMoney(int addVal)
    {
        money += addVal;
    }
}
