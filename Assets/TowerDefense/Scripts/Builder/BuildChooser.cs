using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildChooser : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPurchasePrefab;
    private GameObject towerPurchaseCanvas;
    [SerializeField]
    private float towerXOffset;
    [SerializeField]
    private float towerYOffset;
    [SerializeField]
    private float towerZOffset;
    [SerializeField]
    private float towerXAngleOffset;
    [SerializeField]
    private float towerYAngleOffset;
    [SerializeField]
    private float towerZAngleOffset;

    [SerializeField]
    private GameObject obstaclePurchasePrefab;
    private GameObject obstaclePurchaseCanvas;
    [SerializeField]
    private float obstacleXOffset;
    [SerializeField]
    private float obstacleYOffset;
    [SerializeField]
    private float obstacleZOffset;
    [SerializeField]
    private float obstacleXAngleOffset;
    [SerializeField]
    private float obstacleYAngleOffset;
    [SerializeField]
    private float obstacleZAngleOffset;

    // Start is called before the first frame update
    void Start()
    {
        towerPurchaseCanvas = Instantiate(towerPurchasePrefab);
        towerPurchaseCanvas.transform.parent = gameObject.transform;
        towerPurchaseCanvas.GetComponent<Collider>().enabled = false;
        towerPurchaseCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        obstaclePurchaseCanvas = Instantiate(obstaclePurchasePrefab);
        obstaclePurchaseCanvas.transform.parent = gameObject.transform;
        obstaclePurchaseCanvas.GetComponent<Collider>().enabled = false;
        obstaclePurchaseCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Vector3 towerRot = new Vector3(towerXAngleOffset, towerYAngleOffset, towerZAngleOffset);
    //    towerPurchaseCanvas.transform.localRotation = Quaternion.Euler(towerRot);
    //    towerPurchaseCanvas.transform.localPosition = new Vector3(towerXOffset, towerYOffset, towerZOffset);

    //    Vector3 obstacleRot = new Vector3(obstacleXAngleOffset, obstacleYAngleOffset, obstacleZAngleOffset);
    //    obstaclePurchaseCanvas.transform.localRotation = Quaternion.Euler(obstacleRot);
    //    obstaclePurchaseCanvas.transform.localPosition = new Vector3(obstacleXOffset, obstacleYOffset, obstacleZOffset);
    //}
}
