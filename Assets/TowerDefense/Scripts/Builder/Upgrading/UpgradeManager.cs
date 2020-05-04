using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    void Start()
    {
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

        windCanvas.transform.position = new Vector3(pos.position.x, 8.4f, pos.position.z + 1.05f);
        fireCanvas.transform.position = new Vector3(pos.position.x, 8.4f, pos.position.z - 1.05f);
        fireCanvas.transform.Rotate(new Vector3(0, 180, 0));
        iceCanvas.transform.position = new Vector3(pos.position.x - 1.05f, 8.4f, pos.position.z);
        iceCanvas.transform.Rotate(new Vector3(0, 270, 0));
        electricCanvas.transform.position = new Vector3(pos.position.x + 1.05f, 8.4f, pos.position.z);
        electricCanvas.transform.Rotate(new Vector3(0, 90, 0));
    }


    void Update()
    {
        if (buildManager.BuildModeOn && GoodPosition())
        {
            if (!canvasEnabled && !upgradePanelsActive)
            {
                EnableUpgradeCanvases();
                NoneSelected();
            }

            if (Input.GetKeyDown(KeyCode.X))
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
                }
            }
        }
        else if (canvasEnabled)
        {
            DisableUpgradeCanvases();
        }
    }

    public void UpgradeTower(int elementIndex, int upgradeCost)
    {
        thisChunk.GetComponent<Chunk>().UpgradeTower(elementIndex);
        buildManager.DecreaseMoney(upgradeCost);
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

    /// <summary>
    /// Sets field 'selected' of every canvas on this tower to false and 'selected' of the canvas that called this function to true.
    /// </summary>
    public void Selected(Canvas selectedCanvas)
    {
        NoneSelected();
        TowerUpgrade selectedCanvasTowerUpgrade = selectedCanvas.GetComponent<TowerUpgrade>();
        selectedCanvasTowerUpgrade.setSelectedTrue();
        if (buildManager.money >= selectedCanvasTowerUpgrade.upgradeCost)
        {
            HighlightPanel(selectedCanvas, Color.green);
        } else
        {
            HighlightPanel(selectedCanvas, Color.red);
        }
            
        
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
