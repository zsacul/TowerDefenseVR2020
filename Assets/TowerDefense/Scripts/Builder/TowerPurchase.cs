using UnityEngine;
using System.Collections;

public class TowerPurchase : MonoBehaviour {
    BuildManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        buildManager.ChooseTower();
    }
}
