using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    private BuildModeManager buildManager;
    private BoxCollider thisBoxCollider;
    void Start()
    {
        buildManager= GameObject.Find("GameManager").GetComponent<BuildModeManager>();
        thisBoxCollider = gameObject.AddComponent<BoxCollider>();
        thisBoxCollider.enabled = false;
    }

    void Update()
    {
        if (buildManager.UpdateModeCond())
        {
            thisBoxCollider.enabled = buildManager.BuildModeOn;
        }
    }

    void OnMouseDown()
    {

        // Checking if BuildMode is switched on
        if (buildManager.BuildModeOn)
        {
            //selectedBuildingData consists of selected Building as ChunkType and its cost as int
            System.Tuple<ChunkType, int> selectedBuildingData = buildManager.ActiveBuildingInfo;
            ChunkType building = selectedBuildingData.Item1;
            int money = selectedBuildingData.Item2;
            Debug.Log("PRESSED ON TILE. SELECTED BUILDING = " + building.ToString());
            if (buildManager.Money >= money)
            {
                if (gameObject.GetComponent<Chunk>().ChangeType(building))
                {
                    buildManager.DecreaseMoney(selectedBuildingData.Item2);
                    buildManager.Success();
                    Debug.Log("SUCCESS");
                }
                else
                {
                    buildManager.Failure();
                    Debug.Log("FAILURE");
                }
            }
        }
    }
}
