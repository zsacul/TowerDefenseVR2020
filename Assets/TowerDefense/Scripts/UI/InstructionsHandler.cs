using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsHandler : MonoBehaviour
{
    private Text instructions;

    void Start()
    {
        instructions = this.GetComponent<Text>();
        instructions.text = "Welcome to the tutorial!\nPress .... to open the panel with available buildings and choose a tower";
    }

    public void PanelOpened()
    {
        instructions.text = "Now build this tower anywhere you want"; 
    }

    public void TowerBuild()
    {
        instructions.text = "Great job! You can teleport on the tower to see possible upgrades.\n Choose any update by clicking the panel, and hit the button to confirm your choice";
    }

    void Update()
    {
        
    }
}
