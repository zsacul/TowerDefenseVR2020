using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTowerLigthingAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject lightning;
    [SerializeField]
    GameObject ligtningMaker;
    [SerializeField]
    List<GameObject> lightningEnd;
    [SerializeField]
    List<GameObject> insideLightnings;    
    [SerializeField]
    List<GameObject> MovingLightningsUp;
    [SerializeField]
    List<GameObject> MovingLightningsDown;
    [SerializeField]
    float turnOnLightingsTime;

    private void Start()
    {
        Invoke("MakeLightnings", turnOnLightingsTime);
    } 

    private void MakeLightnings()
    {
        foreach (var l in lightningEnd)
        {
            createLightning(ligtningMaker, l);
        }
        int s = MovingLightningsDown.ToArray().Length;
        for (int i = 0; i < s; i++)
        {
            GameObject l1 = createLightning(ligtningMaker, MovingLightningsUp[i]);
            GameObject l2 = createLightning(MovingLightningsUp[i], MovingLightningsDown[i]);
            l2.transform.parent = l1.transform;
            l2.GetComponent<LightningVisibility>().ChangeVisibility = false;
            l1.GetComponent<LightningVisibility>().childLightning = l2.GetComponent<LineRenderer>();
            l1.GetComponent<LightningVisibility>().VisibleTimeRange = new Vector2(2000, 3000);
        }
    }

    GameObject createLightning(GameObject start, GameObject end)
    {
        //       Debug.Log("powstaje blyskawica");
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        instLightning.transform.parent = this.transform;

        //        Debug.Log("prametry wchodza");
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = start;
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = end;
        instLightning.GetComponent<LightningVisibility>().VisibleTimeRange = new Vector2(100, 400);
        return instLightning;

    }
}
