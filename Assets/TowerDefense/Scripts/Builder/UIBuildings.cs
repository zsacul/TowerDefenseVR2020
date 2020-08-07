using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBuildings : GameEventListener
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject cancelPrefab;

    private GameObject towerInstance;
    private GameObject cancelInstance;
    private BuildManager buildManager;


    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        SetUpUI();
    }

    public void UpdateUIInstances()
    {
        if (buildManager.BuildModeOn)
        {
            if (buildManager.selectedBuilding != ChunkType.none)
            {
                towerInstance.SetActive(false);
                cancelInstance.SetActive(true);
            }
            else
            {
                cancelInstance.SetActive(false);
                towerInstance.SetActive(true);
            }
        }
    }

    public void CloseUI()
    {
        towerInstance.SetActive(false);
        cancelInstance.SetActive(false);
    }

    private void SetUpUI()
    {
        towerInstance = Instantiate(towerPrefab, transform.position, transform.rotation);
        cancelInstance = Instantiate(cancelPrefab, transform.position, transform.rotation);
        towerInstance.transform.parent = gameObject.transform;
        cancelInstance.transform.parent = gameObject.transform;
        //towerInstance.transform.localScale = new Vector3(0.02f, 0.015f, 0.02f);
        //towerInstance.transform.localScale = new Vector3(1f, 1f, 1f);
        cancelInstance.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        towerInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        cancelInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        towerInstance.transform.localPosition = new Vector3(0, -0.02f, -0.015f);
        cancelInstance.transform.localPosition = new Vector3(-0.05f, -0.01f, -0.015f);
        cancelInstance.SetActive(false);
    }

    public override void OnEventRaised(Object data)
    {
        towerInstance.SetActive(!towerInstance.activeSelf);
       // cancelInstance.SetActive(!cancelInstance.active);
    }

    public void OnUITowerClicked()
    {
        //Debug.Log("Inside UIBuildings.OnUITowerClicked()");
        if(buildManager.Selected() == ChunkType.tower)
        {
            buildManager.ChooseNone();
        }
        else
        {
            buildManager.ChooseTower();
        }
    }

}
