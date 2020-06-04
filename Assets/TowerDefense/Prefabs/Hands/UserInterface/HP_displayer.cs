using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_displayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HP_display;
    private GameObject Endpoint;
    private GameObject PoI;
    private Vector3 HP_display_origin;
    private float HP_display_primary_size;
    private float max_hp;
    void Start()
    {
        // fetch needed values.
        HP_display_origin = HP_display.transform.position;
        HP_display_primary_size = HP_display.transform.localScale.y;

        // find Endpoint.
        Endpoint = GameObject.Find("EndPoint");
        // cursed
        PoI = Endpoint.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // since the endpoint might spawn after us, we do stupid crap untill we can find it.
        if (Endpoint == null)
        {
            //Debug.Log("missed");
            // cursed shit. absolutelly cursed shit.
            Endpoint = GameObject.Find("EndPoint(Clone)");
            if (Endpoint != null)
            {
                PoI = Endpoint.transform.GetChild(1).gameObject;
                max_hp = (float)PoI.GetComponent<EndpointManager>().health; // assume that at this point, we have maxhp.
            }
        } else {
            int chp = PoI.GetComponent<EndpointManager>().health;
            float scalechp = ((float)chp) / max_hp; // you can't possibly mean that the maxHP is not 100?
            float newscale = HP_display_primary_size * scalechp;
            //Debug.Log(chp);
            // apply the scale to the HP_display
            Vector3 HPdisplay_scale = HP_display.transform.localScale;
            HPdisplay_scale.y = newscale;
            HP_display.transform.localScale = HPdisplay_scale;
        }
    }
}
