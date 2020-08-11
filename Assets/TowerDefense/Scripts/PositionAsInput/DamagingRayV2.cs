using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagingRayV2 : MonoBehaviour, IChargable
{
    public GameObject projectile;
    public UnityEvent pulse;
    public UnityEvent hit;
    public UnityEvent startPulsing;
    public UnityEvent endPulsing;
    public float repeatRate = 0.5f;
    public float length;

    public void Release()
    {
        StartPulsing();
        Invoke("EndPulsing", length);
    }

    public void SetCharge(float charge)
    {

    }
    void Pulse()
    {
        pulse.Invoke();
        Instantiate(projectile, transform.position, transform.rotation);
    }
    void StartPulsing()
    {
        InvokeRepeating("Pulse", 0, repeatRate);
        startPulsing.Invoke();
    }
    void EndPulsing()
    {
        if (IsInvoking("Pulse"))
        {
            CancelInvoke("Pulse");
            endPulsing.Invoke();
        }
        Destroy(this);
        Destroy(GetComponent<Renderer>());
        Destroy(gameObject, 2);
    }
}
