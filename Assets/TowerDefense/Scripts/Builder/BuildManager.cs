using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
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
    [SerializeField]
    private Canvas towerPurchaseCanvasPrefab;
    [SerializeField]
    private Canvas obstaclePurchaseCanvasPrefab;
    [SerializeField]
    public GameObject rightController;
    [SerializeField]
    private GameEvent WaveChanged;
    [SerializeField]
    private GameEvent BuildingSwitchedToNone; 
    [SerializeField]
    private Text UIMoneyText;
    [SerializeField]
    public bool VRTKInputs;

    private Canvas towerPurchaseCanvas;
    private Canvas obstaclePurchaseCanvas;
    private BoxCollider towerPurchaseCanvasCollider;
    private BoxCollider obstaclePurchaseCanvasCollider;

    private ChunkType selectedBuilding;

    private bool buildModeOn;
    private bool purchasePanelsActive;

    private RectTransform canvasRT;
    private float canvasXSize;
    private float canvasYSize;
    private float canvasZPos;
    private bool rightTriggerInUse;
    private bool panelButtonPressed;

    void Start()
    {
        panelButtonPressed = false;
        towerPurchaseCanvas = Instantiate(towerPurchaseCanvasPrefab);
        obstaclePurchaseCanvas = Instantiate(obstaclePurchaseCanvasPrefab);
        towerPurchaseCanvasCollider = towerPurchaseCanvas.GetComponent<BoxCollider>();
        obstaclePurchaseCanvasCollider = obstaclePurchaseCanvas.GetComponent<BoxCollider>();
        buildModeOn = true;
        purchasePanelsActive = false;
        rightTriggerInUse = false;
        canvasRT = canvas.GetComponent<RectTransform>();
        canvasXSize = canvasRT.sizeDelta.x / 2.0f;
        canvasYSize = canvasRT.sizeDelta.y / 2.0f;
        canvasZPos = canvasRT.localPosition.z;
        selectedBuilding = ChunkType.none;
        SetMoneyText();
    }

    private void SetMoneyText()
    {
        UIMoneyText.text = "$" + money.ToString();
        SetMoneyOutlineColor(new Color(0.02980483f, 1f, 0f, 0.5019608f));
        UpdateUI();
    }


    // <summary>
    // Creates a new text.x and y determines its position.For x = 0.0, y = 0.0 it would be the center of the canvas,
    // for -1.0, -1.0 lower left corner, for 1.0, 1.0 upper right corner.
    // </summary>
    private Text SetText(float x, float y, string content, float width, float height, Color chosen_col)
    {
        GameObject newGO = new GameObject(content);
        newGO.transform.SetParent(canvas.transform);
        newGO.transform.position = Vector3.zero;

        Text newText = newGO.AddComponent<Text>();
        newText.text = content;
        newText.color = chosen_col;
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
            UpdateButtonState(false);

            if (purchasePanelsActive)
            {
                towerPurchaseCanvas.transform.rotation = Quaternion.LookRotation(towerPurchaseCanvas.transform.position - Camera.main.transform.position);
                obstaclePurchaseCanvas.transform.rotation = Quaternion.LookRotation(obstaclePurchaseCanvas.transform.position - Camera.main.transform.position);
            }

            //Changing color of UIMoneyText Outline if we can/can't afford currently selected building
            if (selectedBuilding != ChunkType.none)
            {
                int cost;
                if (selectedBuilding == ChunkType.tower)
                {
                    cost = towerCost;
                }
                else
                {
                    cost = playerObstacleCost;
                }
                if (money <= cost)
                {
                    SetMoneyOutlineColor(Color.red);
                }
                else
                {
                    SetMoneyOutlineColor(new Color(0.02980483f, 1f, 0f, 0.5019608f));
                }
            } else
            {
                SetMoneyOutlineColor(Color.grey);
            }
        }

        if (UpdatePanelActiveCond())
        {
            purchasePanelsActive = !purchasePanelsActive;
            if (purchasePanelsActive && buildModeOn)
            {
                Vector3 towerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.28f, 0.5f, 1.2f));
                Vector3 obstaclePos = Camera.main.ViewportToWorldPoint(new Vector3(0.72f, 0.5f, 1.2f));
                towerPurchaseCanvas.transform.position = towerPos;
                obstaclePurchaseCanvas.transform.position = obstaclePos;
            }

            UpdateUI();
        }

        if (buildModeOn && selectedBuilding != ChunkType.none)
        {
            RaycastHit hit;
            Vector3 lastChunk = new Vector3(0, 0, 0);
            //If we point at something
            if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 10000))
            {
                if (hit.collider.gameObject.tag == "Chunk" && hit.collider.gameObject.transform.position != lastChunk)
                {
                    hit.collider.gameObject.GetComponent<BuildHandler>().SeeYou();
                    lastChunk = hit.collider.gameObject.transform.position;
                }
            }
        }
    }

    void LateUpdate()
    {
        UpdateButtonState(true);
    }

    private void UpdateButtonState(bool isLateUpdate)
    {
        if (isLateUpdate)
        {
            panelButtonPressed = false;
        }
        else
        {
            if (VRTKInputs)
            {
                panelButtonPressed = Input.GetKeyDown(KeyCode.JoystickButton3);
            }
            else
            {
                panelButtonPressed = Input.GetKeyDown(KeyCode.B);
            }
        }
    }

    private void SetMoneyOutlineColor(Color color)
    {
        UIMoneyText.GetComponent<Outline>().effectColor = color;
    }

    //Updates UI according to the state of BuildModeOn
    private void UpdateUI()
    {
        UIMoneyText.enabled = buildModeOn;
        if (buildModeOn)
        {
            UpdatePurchasePanels(purchasePanelsActive);
        }
        else
        {
            UpdatePurchasePanels(false);
        }
    }

    private void UpdatePurchasePanels(bool state)
    {
        towerPurchaseCanvas.enabled = state;
        obstaclePurchaseCanvas.enabled = state;
        towerPurchaseCanvasCollider.enabled = state;
        obstaclePurchaseCanvasCollider.enabled = state;
    }

    public void ChooseTower()
    {
        selectedBuilding = ChunkType.tower;
        purchasePanelsActive = false;
        UpdateUI();
    }

    public void ChooseObstacle()
    {
        selectedBuilding = ChunkType.playerObstacle;
        purchasePanelsActive = false;
        UpdateUI();
    }

    public void ChooseNone()
    {
        selectedBuilding = ChunkType.none;
        purchasePanelsActive = false;
        UpdateUI();
        BuildingSwitchedToNone.Raise();
    }

    public bool UpdatePanelActiveCond()
    {
        return panelButtonPressed;
    }

    public bool BuildModeOn
    {
        get
        {
            return buildModeOn;
        }
    }

    public bool PurchasePanelActive
    {
        get
        {
            return purchasePanelsActive;
        }
    }

    public int Money
    {
        get
        {
            return money;
        }
    }

    ///<summary>
    ///Returns selected building as ChunkType and its cost as int
    ///</summary>
    public System.Tuple<ChunkType, int> ActiveBuildingInfo
    {
        get
        {
            int cost = -1;
            if (selectedBuilding == ChunkType.tower)
            {
                cost = towerCost;
            }
            else if (selectedBuilding == ChunkType.playerObstacle)
            {
                cost = playerObstacleCost;
            }
            return new System.Tuple<ChunkType, int>(selectedBuilding, cost);
        }
    }

    public void DecreaseMoney(int decVal)
    {
        money -= decVal;
        UIMoneyText.text = "$" + money.ToString();
    }

    public void AddMoney(int addVal)
    {
        money += addVal;
        UIMoneyText.text = "$" + money.ToString();
    }


    public int GetMoney()
    {
        return money;
    }

    public void Success()
    {
        BuildingSuccess.Raise();
    }

    public void Failure()
    {
        BuildingFailure.Raise();
    }

    public void ChangeWaveStatus(bool newStatus)
    {
        buildModeOn = newStatus;
        selectedBuilding = ChunkType.none;
        UpdateUI();
        WaveChanged.Raise();
    }
}
