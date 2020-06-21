using UnityEngine;
using System.Collections;

public class TowerPurchase : MonoBehaviour
{
    BuildManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            if (buildManager.ActiveBuildingInfo.Item1 == ChunkType.tower)
            {
                buildManager.ChooseNone();
            }
            else
            {
                buildManager.ChooseTower();
            }
        }
    }
}
