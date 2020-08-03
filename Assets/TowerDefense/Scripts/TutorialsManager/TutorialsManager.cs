using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsManager : MonoBehaviour
{
    bool isAnyTutorialRunning;
    // Start is called before the first frame update
    [SerializeField]
    TeleportMainNode TeleportTutorial;
    [SerializeField]
    CrossbowMainNode CrossbowTutorial;
    [SerializeField]
    BuildTutorialManager BuildingTutorial;

    void Start()
    {
        isAnyTutorialRunning = false;
    }

    // Update is called once per frame
    public bool IsAnyTutorialRunning() {
        return isAnyTutorialRunning;
    }

    public void RunTutorial()
    {
        isAnyTutorialRunning = true;
    }

    public void EndTutorial()
    {
        isAnyTutorialRunning = false;
    }

}
