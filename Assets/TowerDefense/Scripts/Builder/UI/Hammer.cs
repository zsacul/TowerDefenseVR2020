using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * 10);
    }
}
