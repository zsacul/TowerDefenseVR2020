#define DEBUG
//#undef DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanRotation : MonoBehaviour
{
    [SerializeField]
    float RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
    }

    public void speedupRotation(float speedup)
    {
#if DEBUG
        Debug.Log("Predkosc przed przyspieszeniem: " + RotationSpeed);
#endif
        RotationSpeed *= speedup;
#if DEBUG
        Debug.Log("Predkosc po przyspieszeniu o " + speedup + ": " + RotationSpeed);
#endif
    }
}
