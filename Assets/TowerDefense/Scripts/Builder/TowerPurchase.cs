using UnityEngine;
using System.Collections;

public class TowerPurchase : MonoBehaviour {
    BuildModeManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildModeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //towerPurchaseCanvasCollider.enabled = false;
        buildManager.ChooseTower();
    }
}
