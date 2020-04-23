using UnityEngine;
using System.Collections;

public class ObstaclePurchase : MonoBehaviour {
    BuildModeManager buildManager;
    void Start()
    {
        buildManager = GameObject.Find("GameManager").GetComponent<BuildModeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        buildManager.ChooseObstacle();
    }
}
