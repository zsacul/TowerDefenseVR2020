using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    private int elementIndex;
    public int upgradeCost;


    //Indicates if this upgrade was selected by the player
    private bool selected;
    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = GetComponentInParent<UpgradeManager>();
        selected = false;
    }

    void Update()
    {
        if (selected && Input.GetKeyDown(KeyCode.U))
        {
            upgradeManager.UpgradeTower(elementIndex, upgradeCost);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            upgradeManager.Selected(gameObject.GetComponent<Canvas>());
        }
    }

    public void setSelectedFalse()
    {
        selected = false;
        //przywroc kolor
    }

    public void setSelectedTrue()
    {
        selected = true;
        //ustaw kolor taki
    }
}
