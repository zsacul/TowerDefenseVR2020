using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTower : MonoBehaviour
{
    
    [SerializeField]
    public Material m;
    [SerializeField]
    public float disappearSpeed = 1f;
    [SerializeField]
    float disappearAcceleration = 0.01f;
    [SerializeField]
    GameObject fire;

    private float invisiblePart = -0.01f;
    private bool shouldBurn = false;

    private void Start()
    {
        invisiblePart = -0.01f;
    }

    void Update()
    {
        if (shouldBurn)
        {
            m.SetFloat("Vector1_5D8993E", invisiblePart);
            invisiblePart += Time.deltaTime * disappearSpeed;
            disappearSpeed += Time.deltaTime * disappearAcceleration;
            disappearAcceleration += (disappearAcceleration * Time.deltaTime * 10) * (disappearAcceleration * Time.deltaTime * 10);
            if(invisiblePart >= 1.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartBurningTower()
    {
        fire.SetActive(true);
        GetComponent<MeshRenderer>().material = m;
        shouldBurn = true;
    }
}
