using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecialisedUpgradeButton : GenericButton
{
    public GameEvent UpgradeButtonPushed;
    public override void OnButtonPush()
    {
        UpgradeButtonPushed.Raise();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
