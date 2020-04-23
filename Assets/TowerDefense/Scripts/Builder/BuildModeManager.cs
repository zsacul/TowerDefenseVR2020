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
    [SerializeField]
    private Canvas towerPurchaseCanvasPrefab;
    [SerializeField]
    private Canvas obstaclePurchaseCanvasPrefab;
    [SerializeField]
    public GameObject rightController;

    private Canvas towerPurchaseCanvas;
    private Canvas obstaclePurchaseCanvas;
    private BoxCollider towerPurchaseCanvasCollider;
    private BoxCollider obstaclePurchaseCanvasCollider;
    private Text moneyText;

    private ChunkType selectedBuilding;

    private bool buildModeOn;
    private bool purchasePanelActive;

    private RectTransform canvasRT;
    private float canvasXSize;
    private float canvasYSize;
    private float canvasZPos;

    void Start()
    {
        towerPurchaseCanvas = Instantiate(towerPurchaseCanvasPrefab);
        obstaclePurchaseCanvas = Instantiate(obstaclePurchaseCanvasPrefab);
        towerPurchaseCanvasCollider = towerPurchaseCanvas.GetComponent<BoxCollider>();
        obstaclePurchaseCanvasCollider = obstaclePurchaseCanvas.GetComponent<BoxCollider>();
        buildModeOn = false;
        purchasePanelActive = false;
        canvasRT = canvas.GetComponent<RectTransform>();
        canvasXSize = canvasRT.sizeDelta.x / 2.0f;
        canvasYSize = canvasRT.sizeDelta.y / 2.0f;
        canvasZPos = canvasRT.localPosition.z;
        selectedBuilding = ChunkType.none;
        SetMoneyText();
    }

    private void SetMoneyText()
    {
        moneyText = SetText(-0.7f, -0.85f, "Money", 150, 40);
        moneyText.text = "Money: $" + money.ToString();
        UpdateUI();
    }


    // <summary>
    // Creates a new text.x and y determines its position.For x = 0.0, y = 0.0 it would be the center of the canvas,
    // for -1.0, -1.0 lower left corner, for 1.0, 1.0 upper right corner.
    // </summary>
    private Text SetText(float x, float y, string content, float width, float height)
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
            if (purchasePanelActive)
            {
                towerPurchaseCanvas.transform.rotation = Quaternion.LookRotation(towerPurchaseCanvas.transform.position - Camera.main.transform.position);
                obstaclePurchaseCanvas.transform.rotation = Quaternion.LookRotation(obstaclePurchaseCanvas.transform.position - Camera.main.transform.position);
            }
        }

        if (UpdateModeCond())
        {
            buildModeOn = !buildModeOn;
            selectedBuilding = ChunkType.none;

            UpdateUI();
        }

        if (UpdatePanelActiveCond())
        {
            purchasePanelActive = !purchasePanelActive;
            if (purchasePanelActive && buildModeOn)
            {
                Vector3 towerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.28f, 0.3f, 1.4f));
                Vector3 obstaclePos = Camera.main.ViewportToWorldPoint(new Vector3(0.72f, 0.3f, 1.4f));
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
               // Debug.Log("OBJECT HIT: " + hit.collider.gameObject.ToString());
                if (hit.collider.gameObject.tag == "Chunk" && hit.collider.gameObject.transform.position != lastChunk)
                {
                    hit.collider.gameObject.GetComponent<BuildHandler>().SeeYou();
                    lastChunk = hit.collider.gameObject.transform.position;
                }
            }
        }
    }

    //Updates UI according to the state of BuildModeOn
    private void UpdateUI()
    {
        moneyText.enabled = buildModeOn;
        if (buildModeOn)
        {
            UpdatePurchasePanels(purchasePanelActive);
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
        //Debug.Log("Tower was chosen");
        selectedBuilding = ChunkType.tower;
        purchasePanelActive = false;
        UpdateUI();
    }

    public void ChooseObstacle()
    {
        //Debug.Log("Obstacle was chosen");
        selectedBuilding = ChunkType.playerObstacle;
        purchasePanelActive = false;
        UpdateUI();
    }

    // Currently BuildingMode is switched on/off after hitting Tab
    public bool UpdateModeCond()
    {
        return Input.GetKeyDown(KeyCode.Tab);
    }

    public bool UpdatePanelActiveCond()
    {
        return Input.GetKeyDown(KeyCode.B);
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
            return purchasePanelActive;
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
