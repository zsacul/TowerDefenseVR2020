using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;
using System.Linq;


public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private int money;
    public int towerCost;
    public int playerObstacleCost;
    public int fireTowerCost;
    public int iceTowerCost;
    public int electricTowerCost;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameEvent towerBuilt;
    [SerializeField]
    private GameEvent obstacleBuilt;
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
    private GameObject UIMoneyTextGO;
    [SerializeField]
    private GameEvent towerSelected;
    [SerializeField]
    private GameEvent obstacleSelected;
    [SerializeField]
    private GameEvent SelectionStatusChanged;
    [NonSerialized]
    public ChunkType selectedBuilding;
    public bool BuildModeOn { get; private set; }

    private Canvas towerPurchaseCanvas;
    private Canvas obstaclePurchaseCanvas;
    private BoxCollider towerPurchaseCanvasCollider;
    private BoxCollider obstaclePurchaseCanvasCollider;
    private bool purchasePanelsActive;
    private RectTransform canvasRT;
    private float canvasXSize;
    private float canvasYSize;
    private float canvasZPos;
    private bool rightTriggerInUse;
    private bool uiTowerClicked;

    private TextMeshPro UIMoneyText;
    private bool sceneLoaded;
    private static BuildManager instance;
    public ParticleSystem moneyParticle;
    public AudioClip buyClip;
    public AudioClip getMoneyClip;

    public bool PurchasePanelActive {
        get {
            return purchasePanelsActive;
        }
    }

    public int Money {
        get {
            return money;
        }
    }
    
    private void Awake()
    {
        instance = this;
    }

    public static BuildManager Instance {
        get {
            if (instance == null)
            {
                Debug.LogError("Missing BuildManager");
            }
            return instance;
        }
    }
    void Start()
    {
        uiTowerClicked = false;
        BuildModeOn = true;
        rightTriggerInUse = false;
        selectedBuilding = ChunkType.none;
        SetCanvasUI();
        SetMoneyText();
        moneyParticle = Instantiate(moneyParticle.gameObject, transform.position, Quaternion.identity, null).GetComponent<ParticleSystem>();

        //Waiting to load a scene because during loading there's a collision with UITower occuring that causes 
        //canvases to be activated
        sceneLoaded = false;
        StartCoroutine(WaitForSceneLoad(0.5f));
    }

    IEnumerator WaitForSceneLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        sceneLoaded = true;
        uiTowerClicked = false;
        DisableObstacleDestruction();
    }

    private void DisableObstacleDestruction()
    {
        var obstacles = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "IceSpikesObstacle(Clone)");
        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle.GetComponent<BuildingDestroyer>());
        }
    }

    private void SetMoneyText()
    {
        UIMoneyText = UIMoneyTextGO.GetComponent<TextMeshPro>();
        UIMoneyText.text = "$" + money.ToString();
        SetMoneyOutlineColor(new Color(0.02980483f, 1f, 0f, 0.5019608f));
        UIMoneyTextGO.SetActive(BuildModeOn);
    }

    private void SetCanvasUI()
    {
        towerPurchaseCanvas = Instantiate(towerPurchaseCanvasPrefab);
        obstaclePurchaseCanvas = Instantiate(obstaclePurchaseCanvasPrefab);
        towerPurchaseCanvasCollider = towerPurchaseCanvas.GetComponent<BoxCollider>();
        obstaclePurchaseCanvasCollider = obstaclePurchaseCanvas.GetComponent<BoxCollider>();
        purchasePanelsActive = false;
        canvasRT = canvas.GetComponent<RectTransform>();
        canvasXSize = canvasRT.sizeDelta.x / 2.0f;
        canvasYSize = canvasRT.sizeDelta.y / 2.0f;
        canvasZPos = canvasRT.localPosition.z;
        //Debug.Log("Before calling UpdatePurchasePanels(false)");
        UpdatePurchasePanels(false);
    }

    void Update()
    {
        if (BuildModeOn)
        {
            //UpdateButtonState(false);

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
            }

            else
            {
                SetMoneyOutlineColor(Color.grey);
            }
        }

        if (UpdatePanelCondition())
        {
            purchasePanelsActive = !purchasePanelsActive;
            if (purchasePanelsActive && BuildModeOn)
            {
                //Vector3 towerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.28f, 0.5f, 1.2f));
                //Vector3 obstaclePos = Camera.main.ViewportToWorldPoint(new Vector3(0.72f, 0.5f, 1.2f));
                Vector3 towerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.24f, 0.5f, 0.6f));
                Vector3 obstaclePos = Camera.main.ViewportToWorldPoint(new Vector3(0.76f, 0.5f, 0.6f));
                towerPurchaseCanvas.transform.position = new Vector3(towerPos.x, Camera.main.transform.position.y, towerPos.z);
                obstaclePurchaseCanvas.transform.position = new Vector3(obstaclePos.x, Camera.main.transform.position.y, obstaclePos.z);
            }

            //Debug.Log("Before calling UpdateUI()");
            UpdateUI();
        }

        if (BuildModeOn && selectedBuilding != ChunkType.none)
        {
            RaycastHit hit;
            Vector3 lastChunk = new Vector3(0, -99, 0);
            //If we point at something
            if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 10000, ~(1 << 15)))
            {
                if (hit.collider.gameObject.tag == "Chunk" && hit.collider.gameObject.transform.position != lastChunk)
                {
                    hit.collider.gameObject.GetComponent<BuildHandler>().SeeYou();
                    lastChunk = hit.collider.gameObject.transform.position;
                }
            }
        }
    }

    public void UITowerClicked()
    {
        uiTowerClicked = !uiTowerClicked;
    }

    private void SetMoneyOutlineColor(Color color)
    {
        UIMoneyText.outlineColor = color;
        //UIMoneyText.GetComponent<Outline>().effectColor = color;
    }

    //Updates UI according to the state of BuildModeOn
    private void UpdateUI()
    {
        UIMoneyTextGO.SetActive(BuildModeOn);
        if (BuildModeOn)
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
        // Debug.Log("UpdatePurchasePanels called with argument state = " + state.ToString());
        towerPurchaseCanvas.enabled = state;
        obstaclePurchaseCanvas.enabled = state;
        towerPurchaseCanvasCollider.enabled = state;
        obstaclePurchaseCanvasCollider.enabled = state;
    }

    public void ChooseTower()
    {
        if (BuildModeOn)
        {
            //StartedPointing.Invoke();
            RightRaycast.Instance.TurnBuilding(true);
            RightRaycast.Instance.TurnTeleport(false);
            towerSelected.Raise();
            selectedBuilding = ChunkType.tower;
            purchasePanelsActive = false;
            UpdateUI();
        }
    }

    public void ChooseObstacle()
    {
        if (BuildModeOn)
        {
            //StartedPointing.Invoke();
            RightRaycast.Instance.TurnBuilding(true);
            RightRaycast.Instance.TurnTeleport(false);
            obstacleSelected.Raise();
            selectedBuilding = ChunkType.playerObstacle;
            purchasePanelsActive = false;
            UpdateUI();
        }
    }

    public void ChooseNone()
    {
        //StoppedPointing.Invoke();
        RightRaycast.Instance.TurnBuilding(false);
        RightRaycast.Instance.TurnTeleport(true);
        selectedBuilding = ChunkType.none;
        purchasePanelsActive = false;
        UpdateUI();
        BuildingSwitchedToNone.Raise();
    }


    private bool UpdatePanelCondition()
    {
        if (!sceneLoaded)
        {
            return false;
        }

        if (uiTowerClicked)
        {
            uiTowerClicked = !uiTowerClicked;
            return true;
        }

        // return Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.B);
        return false;
    }

    ///<summary>
    ///Returns selected building as ChunkType and its cost as int
    ///</summary>
    public System.Tuple<ChunkType, int> ActiveBuildingInfo {
        get {
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
        AudioSource.PlayClipAtPoint(buyClip, Camera.main.transform.position);
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
        if(selectedBuilding == ChunkType.tower)
        {
            towerBuilt.Raise();
        }
        else if(selectedBuilding == ChunkType.playerObstacle)
        {
            obstacleBuilt.Raise();
        }
    }

    public void Failure()
    {
        BuildingFailure.Raise();
    }

    public void ChangeWaveStatus(bool newStatus)
    {
        BuildModeOn = newStatus;
        ChooseNone();
        WaveChanged.Raise();
    }

    public ChunkType Selected()
    {
        return selectedBuilding;
    }
    public static void BuildTower()
    {
        instance.ChooseTower();
    }
    public static void BuildObstacle()
    {
        instance.ChooseObstacle();
    }
    public static void ClearBuildMode()
    {
        instance.ChooseNone();
    }
    public static bool CanBuildTower()
    {
        return instance.money >= instance.towerCost && BuildingConditions.Instance.towerAnywhere;
    }
    public static bool CanBuildObstacle()
    {
        return (instance.money >= instance.playerObstacleCost) && (BuildingConditions.Instance.obstacleOnPath || BuildingConditions.Instance.obstacleAnywhere);
    }
    public static void BurstMoneyEffect(int amount, Vector3 position)
    {
        instance.moneyParticle.transform.position = position;
        instance.moneyParticle.Emit(amount);
    }
}
