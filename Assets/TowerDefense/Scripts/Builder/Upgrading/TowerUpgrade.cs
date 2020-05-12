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

    void Start()
    {
        upgradeButtonPressed = false;
        upgradeManager = GetComponentInParent<UpgradeManager>();
        selected = false;
    }

    void Update()
    {
        UpdateButtonState(false);
        if (selected && upgradeButtonPressed)//Input.GetKeyDown(KeyCode.U))
        {
            upgradeManager.UpgradeTower(elementIndex, upgradeCost);
        }
    }
    void LateUpdate()
    {
        UpdateButtonState(true);
    }

    private void UpdateButtonState(bool isLateUpdate)
    {
        if (isLateUpdate)
        {
            upgradeButtonPressed = false;
        }
        else
        {
            upgradeButtonPressed = Input.GetKeyDown(KeyCode.JoystickButton0);
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
    }

    public void setSelectedTrue()
    {
        selected = true;
    }
}
