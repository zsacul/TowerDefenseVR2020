using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildings : GameEventListener
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private GameObject obstaclePrefab;

    private GameObject towerInstance;
    private GameObject obstacleInstance;
    private BuildManager buildManager;


    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        SetUpUI();
    }

    private void SetUpUI()
    {
        towerInstance = Instantiate(towerPrefab, transform.position, transform.rotation);
        obstacleInstance = Instantiate(obstaclePrefab, transform.position, transform.rotation);
        towerInstance.transform.parent = gameObject.transform;
        obstacleInstance.transform.parent = gameObject.transform;
        towerInstance.transform.localScale = new Vector3(0.02f, 0.013f, 0.02f);
        obstacleInstance.transform.localScale = new Vector3(0.015f, 0.03f, 0.015f);
        towerInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        obstacleInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        towerInstance.transform.localPosition = new Vector3(0, -0.02f, -0.015f);
        obstacleInstance.transform.localPosition = new Vector3(-0.067f, -0.02f, 0.044f);
    }

    public override void OnEventRaised(Object data)
    {
        towerInstance.SetActive(false);
        obstacleInstance.SetActive(false);
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

    public void OnUIObstacleClicked()
    {
        if(buildManager.Selected() == ChunkType.playerObstacle)
        {
            buildManager.ChooseNone();
        }
        else
        {
            buildManager.ChooseObstacle();
        }
    }

}
