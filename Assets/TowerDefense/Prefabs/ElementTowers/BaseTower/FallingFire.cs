using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFire : MonoBehaviour
{
    [SerializeField]
    public GameObject Fire;
    [SerializeField]
    public float FallingSpeed = 1f;
    [SerializeField]
    public float RadiusSpeed = 1f;

    private bool radiusSwitch = false;

    void FixedUpdate()
    {
        
        if (Fire.transform.position.y > 0f)
        {
            Fire.transform.position = new Vector3(Fire.transform.position.x, Fire.transform.position.y - FallingSpeed, Fire.transform.position.z);
            
                Fire.transform.localScale = new Vector3(Fire.transform.localScale.x - RadiusSpeed,  Fire.transform.localScale.y - RadiusSpeed, 1f);
            
        }
    }
}
