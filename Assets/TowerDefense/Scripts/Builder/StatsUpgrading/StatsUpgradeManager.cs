using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatsUpgradeManager : MonoBehaviour
{
    [SerializeField]
    private Canvas statsUpgradeCanvasPrefab;
    private Canvas currentLevelCanvas;
    private Canvas nextLevelCanvas;
    private BaseTower towerScript;
    private BuildManager buildManager;
    private Transform pos;
    private float damage;
    private float nextLevelDamage;
    private int damagePerLevel;
    private float range;
    private float nextLevelRange;
    private int rangePerLevel;
    private float speed;
    private float nextLevelSpeed;
    private int speedPerLevel;
    private int currentLevel;
    private int nextLevelCostIndex;
    private bool panelsActive;
    private bool canvasEnabled;

    //private GameEvent LevelUpSelected;
    [SerializeField]
    private GameEvent LevelUpSuccess;
    //private GameEvent LevelUpFailure;

    [SerializeField]
    int[] upgradeCosts;
    [SerializeField]
    private GameObject buttonPrefab;
    private GameObject buttonInstance;
    private int maxLevel;


    private Color neutralColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    private Color upgradeColor = new Color(0f, 1f, 0.11f, 1f);
    void Start()
    {
        towerScript = gameObject.GetComponent<BaseTower>();
        buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
        currentLevelCanvas = Instantiate(statsUpgradeCanvasPrefab);
        currentLevelCanvas.transform.parent = transform;
        nextLevelCanvas = Instantiate(statsUpgradeCanvasPrefab);
        nextLevelCanvas.transform.parent = transform;
        damage = towerScript.DamageStatInfo().Item1;
        damagePerLevel = towerScript.DamageStatInfo().Item2;
        range = towerScript.RangeStatInfo().Item1;
        rangePerLevel = towerScript.RangeStatInfo().Item2;
        speed = towerScript.DelayStatInfo().Item1;
        speedPerLevel = towerScript.DelayStatInfo().Item2;
        currentLevel = 1;
        maxLevel = upgradeCosts.Length + 1;
        nextLevelCostIndex = 0;
        panelsActive = false;

        pos = transform;
        currentLevelCanvas.transform.position = new Vector3(pos.position.x - 0.45f, 8.65f, pos.position.z + 1.05f);
        currentLevelCanvas.GetComponent<Collider>().enabled = false;
        nextLevelCanvas.transform.position = new Vector3(pos.position.x + 0.45f, 8.65f, pos.position.z + 1.05f);

        HighlightPanel(currentLevelCanvas, Color.clear);
        UpdatePanels();
        DisablePanels();

        buttonInstance = Instantiate(buttonPrefab);
        SetButtonPosition(nextLevelCanvas);
    }

    private void SetButtonPosition(Canvas selectedCanvas)
    {
        Vector3 rot = new Vector3(270, 45, 0);
        buttonInstance.SetActive(true);
        buttonInstance.transform.SetParent(selectedCanvas.transform, false);
        buttonInstance.transform.localPosition = new Vector3(0.704f, -0.102f, -0.102f);
        buttonInstance.transform.localRotation = Quaternion.Euler(rot);
    }

    void Update()
    {
        //Debug.Log("Problem is in Update");
        if (buildManager.BuildModeOn && GoodPosition())
        {
            if (!canvasEnabled && !panelsActive)
            {
                EnablePanels();
            }

            if (buildManager.VRTKInputs && Input.GetKeyDown(KeyCode.JoystickButton1) ||
                (!buildManager.VRTKInputs && Input.GetKeyDown(KeyCode.X)))
            {
                panelsActive = !panelsActive;
                if (canvasEnabled)
                {
                    DisablePanels();
                } else
                {
                    EnablePanels();
                }
            }
        }
        else if (canvasEnabled)
        {
            DisablePanels();
        }
    }

    void SetCurrentLevelCanvas()
    {
        TextMeshProUGUI levelInfo = currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI damageInfo = currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI rangeInfo = currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[2];
        TextMeshProUGUI speedInfo = currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[3];
        currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[5].enabled = false;
        currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[6].enabled = false;
        currentLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[7].SetText("Press \"X\" to dismiss panels");
        currentLevelCanvas.GetComponent<Collider>().enabled = false;

        if (currentLevel == maxLevel)
        {
            currentLevelCanvas.transform.position = new Vector3(pos.position.x, 8.7f, pos.position.z + 1.05f);
            levelInfo.SetText("Tower level: Max");
        } else
        {
            levelInfo.SetText("Tower level: {0}", currentLevel);
        }

        damageInfo.SetText(damage.ToString());
        damageInfo.color = neutralColor;
        rangeInfo.SetText(range.ToString());
        rangeInfo.color = neutralColor;
        speedInfo.SetText(speed.ToString());
        speedInfo.color = neutralColor;
    }

    void SetNextLevelCanvas()
    {
        TextMeshProUGUI levelInfo  = nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI damageInfo = nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI rangeInfo = nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[2];
        TextMeshProUGUI speedInfo = nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[3];
        TextMeshProUGUI costInfo = nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[5];

        nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[6].enabled = true;
        nextLevelCanvas.GetComponentsInChildren<TextMeshProUGUI>()[7].enabled = true;

        costInfo.color = (buildManager.Money >= upgradeCosts[nextLevelCostIndex]) ? Color.green : Color.red;
        costInfo.SetText("Cost: {0}", upgradeCosts[nextLevelCostIndex]);

        levelInfo.SetText("Next level: {0}", currentLevel + 1);
        nextLevelDamage = (float)Math.Round(damage * (1f + damagePerLevel / 100f), 2);
        damageInfo.SetText(nextLevelDamage.ToString());
        damageInfo.color = upgradeColor;
        nextLevelRange = (float)Math.Round(range * (1f + rangePerLevel / 100f), 2);
        rangeInfo.SetText(nextLevelRange.ToString());
        rangeInfo.color = upgradeColor;
        nextLevelSpeed = (float)Math.Round(speed * (1f - speedPerLevel / 100f), 2);
        speedInfo.SetText(nextLevelSpeed.ToString());
        speedInfo.color = upgradeColor;
    }

    public void TowerLevelUp()
    {
        if ((currentLevel != maxLevel) && (buildManager.GetMoney() >= upgradeCosts[nextLevelCostIndex]))
        {
            LevelUpSuccess.Raise();
            buildManager.DecreaseMoney(upgradeCosts[nextLevelCostIndex]);
            currentLevel += 1;
            towerScript.UpgradeDelay();
            towerScript.UpgradeFireDamage();
            towerScript.UpgradeRange();
            if (currentLevel != maxLevel)
            {
                nextLevelCostIndex += 1;
                UpdateStats();
            }
            NotSelected();
        } else if ((currentLevel != maxLevel) && (buildManager.GetMoney() >= upgradeCosts[nextLevelCostIndex]))
        {
            //LevelUpFailure.Raise();
        }

        UpdatePanels();
    }

    public void UpdatePanels()
    {
        if (currentLevel == maxLevel)
        {
            SetCurrentLevelCanvas();
            nextLevelCanvas.enabled = false;
            nextLevelCanvas.GetComponent<Collider>().enabled = false;
        } else
        {
            SetCurrentLevelCanvas();
            SetNextLevelCanvas();
        }
    }

    public void UpdateStats()
    {
        damage = nextLevelDamage;
        range = nextLevelRange;
        speed = nextLevelSpeed;
    }

    public void Selected()
    {
        //LevelUpSelected.Raise();
        nextLevelCanvas.GetComponent<TowerStatsUpgrade>().setSelectedTrue();
        Color highlightColor = (buildManager.GetMoney() >= upgradeCosts[nextLevelCostIndex]) ? Color.green : Color.red;
        HighlightPanel(nextLevelCanvas, highlightColor);
    }

    public void NotSelected()
    {
        nextLevelCanvas.GetComponent<TowerStatsUpgrade>().setSelectedFalse();
        HighlightPanel(nextLevelCanvas, Color.clear);
    }

    void DisablePanels()
    {
        // Check if buttonInstance is null
        if (buttonInstance)
        {
            buttonInstance.SetActive(false);
        }
        canvasEnabled = false;
        currentLevelCanvas.enabled = false;
        nextLevelCanvas.enabled = false;
        nextLevelCanvas.GetComponent<Collider>().enabled = false;
        NotSelected();
    }

    void EnablePanels()
    {
        // Check if buttonInstance is null
        if (buttonInstance)
        {
            buttonInstance.SetActive(true);
        }
        UpdatePanels();
        canvasEnabled = true;
        currentLevelCanvas.enabled = true;
        nextLevelCanvas.enabled = true;
        nextLevelCanvas.GetComponent<Collider>().enabled = true;
    }

    bool GoodPosition()
    {
        bool goodX, goodZ;
        goodX = Camera.main.transform.position.x >= (transform.position.x - 1) &&
            Camera.main.transform.position.x <= (transform.position.x + 1);
        goodZ = Camera.main.transform.position.z >= (transform.position.z - 1) &&
            Camera.main.transform.position.z <= (transform.position.z + 1);

        return goodX && goodZ;
    }

    private void HighlightPanel(Canvas panel, Color color)
    {
        Image backgroundImage = panel.GetComponentInChildren<Image>();
        backgroundImage.GetComponent<Outline>().effectColor = color;
    }
}
