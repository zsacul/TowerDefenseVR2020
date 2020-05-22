using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private Canvas fireCanvasPrefab;
    [SerializeField]
    private Canvas iceCanvasPrefab;
    [SerializeField]
    private Canvas windCanvasPrefab;
    [SerializeField]
    private Canvas electricCanvasPrefab;

    private Canvas fireCanvas;
    private Canvas iceCanvas;
    private Canvas windCanvas;
    private Canvas electricCanvas;
    private BuildManager buildManager;
    private bool canvasEnabled;
    private Transform pos;
    private GameObject thisChunk;
    private bool upgradePanelsActive;
    private bool panelButtonPressed;
    [SerializeField]
    private GameObject buttonPrefab;
    private GameObject buttonInstance;
    private bool anyUpgradeSelected;

    private GameEvent UpgradeSelected;
    private GameEvent UpgradeSuccess;
    private GameEvent UpgradeFailure;


    void Start()
    {
        panelButtonPressed = false;
        anyUpgradeSelected = false;

        thisChunk = transform.parent.gameObject;
        upgradePanelsActive = false;

        fireCanvas = Instantiate(fireCanvasPrefab);
        fireCanvas.transform.parent = transform;

        iceCanvas = Instantiate(iceCanvasPrefab);
        iceCanvas.transform.parent = transform;

        windCanvas = Instantiate(windCanvasPrefab);
        windCanvas.transform.parent = transform;

        electricCanvas = Instantiate(electricCanvasPrefab);
        electricCanvas.transform.parent = transform;

        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        pos = transform;

        DisableUpgradeCanvases();

        windCanvas.transform.position = new Vector3(pos.position.x, 8.65f, pos.position.z + 1.05f);
        fireCanvas.transform.position = new Vector3(pos.position.x, 8.65f, pos.position.z - 1.05f);
        fireCanvas.transform.Rotate(new Vector3(0, 180, 0));
        iceCanvas.transform.position = new Vector3(pos.position.x - 1.05f, 8.65f, pos.position.z);
        iceCanvas.transform.Rotate(new Vector3(0, 270, 0));
        electricCanvas.transform.position = new Vector3(pos.position.x + 1.05f, 8.65f, pos.position.z);
        electricCanvas.transform.Rotate(new Vector3(0, 90, 0));

        buttonInstance = Instantiate(buttonPrefab);
        buttonInstance.SetActive(false);
    }


    void Update()
    {
        UpdateButtonState(false);

        if (buildManager.BuildModeOn && GoodPosition())
        {
            if (!canvasEnabled && !upgradePanelsActive)
            {
                EnableUpgradeCanvases();
                SetUpgradeCosts();
                buildManager.ChooseNone();
                NoneSelected();
            }

            if (panelButtonPressed)//Input.GetKeyDown(KeyCode.X))
            {
                upgradePanelsActive = !upgradePanelsActive;
                if (canvasEnabled)
                {
                    NoneSelected();
                    DisableUpgradeCanvases();
                }
                else
                {
                    EnableUpgradeCanvases();
                    SetUpgradeCosts();
                }
            }
        }
        else if (canvasEnabled)
        {
            DisableUpgradeCanvases();
            NoneSelected();
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
            if (buildManager.VRTKInputs)
            {
                panelButtonPressed = Input.GetKeyDown(KeyCode.JoystickButton1);
            }
            else
            {
                panelButtonPressed = Input.GetKeyDown(KeyCode.X);
            }
        }
    }

    public void UpgradeTower(int elementIndex, int upgradeCost)
    {
        if (buildManager.GetMoney() >= upgradeCost)
        {
            UpgradeSuccess.Raise();
            buildManager.DecreaseMoney(upgradeCost);
            thisChunk.GetComponent<Chunk>().UpgradeTower(elementIndex);
            SetUpgradeCosts();
        } else
        {
            UpgradeFailure.Raise();
        }
    }

    bool GoodPosition()
    {
        bool goodX, goodZ;
        goodX = Camera.main.transform.position.x >= (transform.position.x - 1) &&
            Camera.main.transform.position.x <= (transform.position.x + 1);
        goodZ = Camera.main.transform.position.z >= (transform.position.z - 1) &&
            Camera.main.transform.position.z <= (transform.position.z + 1);

        return goodX && goodZ;
    }

    private void DisableUpgradeCanvases()
    {
        canvasEnabled = false;
        fireCanvas.enabled = false;
        fireCanvas.GetComponent<Collider>().enabled = false;
        iceCanvas.enabled = false;
        iceCanvas.GetComponent<Collider>().enabled = false;
        windCanvas.enabled = false;
        windCanvas.GetComponent<Collider>().enabled = false;
        electricCanvas.enabled = false;
        electricCanvas.GetComponent<Collider>().enabled = false;
        if (anyUpgradeSelected)
        {
            buttonInstance.SetActive(false);
            anyUpgradeSelected = false; 
        }
    }

    private void EnableUpgradeCanvases()
    {
        canvasEnabled = true;
        fireCanvas.enabled = true;
        fireCanvas.GetComponent<Collider>().enabled = true;
        iceCanvas.enabled = true;
        iceCanvas.GetComponent<Collider>().enabled = true;
        windCanvas.enabled = true;
        windCanvas.GetComponent<Collider>().enabled = true;
        electricCanvas.enabled = true;
        electricCanvas.GetComponent<Collider>().enabled = true;
    }

    private void SetUpgradeCosts()
    {
        int fireUpgradeCost = fireCanvas.GetComponent<TowerUpgrade>().upgradeCost;
        TextMeshProUGUI fireUpgradeCostLabel = fireCanvas.GetComponentInChildren<TextMeshProUGUI>();
        fireUpgradeCostLabel.SetText("Cost: {0}", fireUpgradeCost);
        fireUpgradeCostLabel.color = (buildManager.Money >= fireUpgradeCost) ? Color.green : Color.red;

        int iceUpgradeCost = electricCanvas.GetComponent<TowerUpgrade>().upgradeCost;
        TextMeshProUGUI iceUpgradeCostLabel = iceCanvas.GetComponentInChildren<TextMeshProUGUI>();
        iceUpgradeCostLabel.SetText("Cost: {0}", iceUpgradeCost);
        iceUpgradeCostLabel.color = (buildManager.Money >= iceUpgradeCost) ? Color.green : Color.red;

        int electricUpgradeCost = electricCanvas.GetComponent<TowerUpgrade>().upgradeCost;
        TextMeshProUGUI electricUpgradeCostLabel = electricCanvas.GetComponentInChildren<TextMeshProUGUI>();
        electricUpgradeCostLabel.SetText("Cost: {0}", electricUpgradeCost);
        electricUpgradeCostLabel.color = (buildManager.Money >= electricUpgradeCost) ? Color.green : Color.red;

        int windUpgradeCost = windCanvas.GetComponent<TowerUpgrade>().upgradeCost;
        TextMeshProUGUI windUpgradeCostLabel = windCanvas.GetComponentInChildren<TextMeshProUGUI>();
        windUpgradeCostLabel.SetText("Cost: {0}", windUpgradeCost);
        windUpgradeCostLabel.color = (buildManager.Money >= windUpgradeCost) ? Color.green : Color.red;
    }

    /// <summary>
    /// Sets field 'selected' of every canvas on this tower to false and 'selected' of the canvas that called this function to true.
    /// </summary>
    public void Selected(Canvas selectedCanvas)
    {
        UpgradeSelected.Raise();
        anyUpgradeSelected = true;
        NoneSelected();
        TowerUpgrade selectedCanvasTowerUpgrade = selectedCanvas.GetComponent<TowerUpgrade>();
        selectedCanvasTowerUpgrade.setSelectedTrue();
        Color highlightColor = (buildManager.GetMoney() >= selectedCanvasTowerUpgrade.upgradeCost) ? Color.green : Color.red;
        HighlightPanel(selectedCanvas, highlightColor);


        Vector3 buttonRot = new Vector3(90, 0, 0);
        buttonInstance.SetActive(true);
        buttonInstance.transform.SetParent(selectedCanvas.transform, false);
        buttonInstance.transform.localPosition = new Vector3(0.6f, 0.0f, 0.2f);
    }

    // Sets field 'selected' of every canvas on this tower to false.
    public void NoneSelected()
    {
        fireCanvas.GetComponent<TowerUpgrade>().setSelectedFalse();
        HighlightPanel(fireCanvas, Color.clear);

        iceCanvas.GetComponent<TowerUpgrade>().setSelectedFalse();
        HighlightPanel(iceCanvas, Color.clear);

        windCanvas.GetComponent<TowerUpgrade>().setSelectedFalse();
        HighlightPanel(windCanvas, Color.clear);

        electricCanvas.GetComponent<TowerUpgrade>().setSelectedFalse();
        HighlightPanel(electricCanvas, Color.clear);
    }

    private void HighlightPanel(Canvas panel, Color color)
    {
        Image backgroundImage = panel.GetComponentInChildren<Image>();
        backgroundImage.GetComponent<Outline>().effectColor = color;
    }
}
