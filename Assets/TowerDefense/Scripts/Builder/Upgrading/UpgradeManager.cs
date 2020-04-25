using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        thisChunk = transform.parent.gameObject;
        fireCanvas = Instantiate(fireCanvasPrefab);
        fireCanvas.transform.parent = transform;
        fireCanvas.GetComponent<TowerUpgrade>().chunk = thisChunk;

        iceCanvas = Instantiate(iceCanvasPrefab);
        iceCanvas.transform.parent = transform;
        iceCanvas.GetComponent<TowerUpgrade>().chunk = thisChunk;

        windCanvas = Instantiate(windCanvasPrefab);
        windCanvas.transform.parent = transform;
        windCanvas.GetComponent<TowerUpgrade>().chunk = thisChunk;

        electricCanvas = Instantiate(electricCanvasPrefab);
        electricCanvas.transform.parent = transform;
        electricCanvas.GetComponent<TowerUpgrade>().chunk = thisChunk;

        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        pos = transform;

        DisableUpgradeCanvases();

        windCanvas.transform.position = new Vector3(pos.position.x, 8.2f, pos.position.z + 1.2f);
        fireCanvas.transform.position = new Vector3(pos.position.x, 8.2f, pos.position.z - 1.2f);
        fireCanvas.transform.Rotate(new Vector3(0, 180, 0));
        iceCanvas.transform.position = new Vector3(pos.position.x - 1.2f, 8.2f, pos.position.z);
        iceCanvas.transform.Rotate(new Vector3(0, 270, 0));
        electricCanvas.transform.position = new Vector3(pos.position.x + 1.2f, 8.2f, pos.position.z);
        electricCanvas.transform.Rotate(new Vector3(0, 90, 0));
    }


    void Update()
    {
        if (buildManager.BuildModeOn && GoodPosition())
        {
            if (!canvasEnabled)
            {
                EnableUpgradeCanvases();
            }
        }
        else if (canvasEnabled)
        {
            DisableUpgradeCanvases();
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
}
