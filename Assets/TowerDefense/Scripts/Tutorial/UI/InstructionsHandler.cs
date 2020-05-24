using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InstructionsHandler : GameEventListener
{
    private Text instructions;
    private bool panelOpenedBefore;
    private bool towerBuiltBefore;
    private bool towerUpgradedBefore;

    public string sceneName;

    void Start()
    {
        panelOpenedBefore = towerBuiltBefore = towerUpgradedBefore = false;
        instructions = this.GetComponent<Text>();
        instructions.text = "Welcome to the tutorial!\nPress left X to open the panel with available buildings and choose a tower";
    }

    public override void OnEventRaised(Object data)
    {
        PanelOpened();
    }

    private void PanelOpened()
    {
        if (!panelOpenedBefore)
        {
            instructions.text = "Now aim with your right hand and click\n left Y to build the tower";
            panelOpenedBefore = true;
        }
    }

    public void TowerBuilt()
    {
        if (!towerBuiltBefore)
        {
            instructions.text = "Great job!\n Teleport on the tower to see possible upgrades.\n Click one of four panels to choose,\n and hit the button to upgrade";
            towerBuiltBefore = true;
        }
    }

    public void TowerUpgraded()
    {
        if (!towerUpgradedBefore)
        {
            instructions.text = "If you want, you can keep upgrading your tower even further.\nYou can also put some obstacles to modify enemies' path.\nIf you think you're ready, press right Y to start your first wave!";
            towerUpgradedBefore = true;
        }
    }

    public void EndOfWave()
    {
        instructions.text = "Congratulations, you have won!\n Now let's start the real challenge.";
        StartCoroutine(endTutorial());
    }

    IEnumerator endTutorial()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneName);
    }


}
