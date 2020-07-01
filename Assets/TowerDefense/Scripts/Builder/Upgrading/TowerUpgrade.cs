using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    private int elementIndex;
    public int upgradeCost;
    private bool upgradeButtonPressed;

    //Indicates if this upgrade was selected by the player
    private bool selected;
    private UpgradeManager upgradeManager;
    private BuildManager buildManager;

    void Start()
    {
        upgradeButtonPressed = false;
        upgradeManager = GetComponentInParent<UpgradeManager>();
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        selected = false;
    }

    void Update()
    {
        //UpdateButtonState(false);
        if (selected && upgradeButtonPressed)//Input.GetKeyDown(KeyCode.U))
        {
            Upgrade();
        }
    }
    /*void LateUpdate()
    {
        UpdateButtonState(true);
    }*/

    public void Upgrade()
    {
        upgradeManager.UpgradeTower(elementIndex, upgradeCost);
    }

    /*private void UpdateButtonState(bool isLateUpdate)
    {
        if (isLateUpdate)
        {
            upgradeButtonPressed = false;
        }
        else
        {
            if (buildManager.VRTKInputs)
            {
                upgradeButtonPressed = Input.GetKeyDown(KeyCode.JoystickButton0);
            }
            else
            {
                upgradeButtonPressed = Input.GetKeyDown(KeyCode.U);
            }
        }
    }*/

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
    }

    public void setSelectedTrue()
    {
        selected = true;
    }
}
