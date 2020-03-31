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
    private Canvas canvas;
    [SerializeField]
    private GameEvent BuildingSuccess;
    [SerializeField]
    private GameEvent BuildingFailure;

    private ChunkType selectedBuilding;
    private bool buildModeOn;
    private Text moneyText;
    private Text playerObstacleText;
    private Text turretText;
    private Text instructionText;
    private RectTransform canvasRT;
    private float canvasXSize;
    private float canvasYSize;
    private float canvasZPos;

    void Start()
    {
        buildModeOn = false;
        canvasRT = canvas.GetComponent<RectTransform>();
        canvasXSize = canvasRT.sizeDelta.x / 2.0f;
        canvasYSize = canvasRT.sizeDelta.y / 2.0f;
        canvasZPos = canvasRT.localPosition.z;
        SetTexts();
    }

    void SetTexts()
    {
        moneyText = SetText(-0.7f, -0.85f, "Money", 150, 40);
        playerObstacleText = SetText(-0.5f, -0.85f, "Press O to choose an Obstacle", 150, 40);
        turretText = SetText(-0.2f, -0.85f, "Press T to choose a Tower", 150, 40);
        instructionText = SetText(0.0f, 0.85f, "Building Mode: press Tab", 250, 40);
        moneyText.text = "Money: $" + money.ToString();
        UpdateUI();
    }


    /// <summary>
    /// Creates a new text. x and y determines its position. For x = 0.0, y = 0.0 it would be the center of the canvas,
    /// for -1.0, -1.0 lower left corner, for 1.0, 1.0 upper right corner.
    /// </summary>
    Text SetText(float x, float y, string content, float width, float height)
    {
        GameObject newGO = new GameObject(content);
        newGO.transform.SetParent(canvas.transform);
        newGO.transform.position = Vector3.zero;

        Text newText = newGO.AddComponent<Text>();
        newText.text = content;
        newText.color = Color.black;
        newText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        newText.fontStyle = FontStyle.Bold;
        newText.fontSize = 15;
        newText.rectTransform.sizeDelta = new Vector2(width, height);
        newText.transform.localPosition = new Vector3(canvasXSize * x, canvasYSize * y, canvasZPos);
        return newText;
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

        }
        if (UpdateModeCond())
        {
            buildModeOn = !buildModeOn;
            selectedBuilding = ChunkType.none;

            UpdateUI();

            if (buildModeOn)
            {
                SetTextColours(Color.black, Color.black);
            }
        }
        
    }

    //Updates UI according to the state of BuildModeOn
    void UpdateUI()
    {
        playerObstacleText.enabled = BuildModeOn;
        moneyText.enabled = BuildModeOn;
        turretText.enabled = BuildModeOn;
    }

    //Sets colours of turretText and playerObstacleText
    void SetTextColours(Color fortower, Color forObstacle)
    {
        turretText.color = fortower;
        playerObstacleText.color = forObstacle;
    }

    // Currently BuildingMode is switched on/off after hitting Tab
    public bool UpdateModeCond()
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
        moneyText.text = "Money: $" + money.ToString();
    }

    public void AddMoney(int addVal)
    {
        money += addVal;
        moneyText.text = "Money: $" + money.ToString();
    }

    public void Success()
    {
        BuildingSuccess.Raise();
    }

    public void Failure()
    {
        BuildingFailure.Raise();
    }
}
