using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningVisibility : MonoBehaviour
{
    
    public Vector2 VisibleTimeRange = new Vector2(3000, 5000);
    public bool ChangeVisibility = true;

    private float changeVisibilityTime = 0;
    private bool visibility = true;
    public LineRenderer childLightning;
    private void Start()
    {
        visibility = UnityEngine.Random.Range(0, 2) == 0 ? false : true;
    }
    void Update()
    {
        if (ChangeVisibility)
        {
            if (changeVisibilityTime <= 0)
            {
                visibility = !visibility;
                GetComponent<LineRenderer>().enabled = visibility;
                if (visibility == false) changeVisibilityTime = UnityEngine.Random.Range(2500, 5000) / 1000f;
                else changeVisibilityTime = UnityEngine.Random.Range(VisibleTimeRange.x, VisibleTimeRange.y) / 1000f;
                if (childLightning != null)
                    childLightning.enabled = visibility;
            }
            else
            {
                changeVisibilityTime -= Time.deltaTime;
            }
        }
    }
    
}
