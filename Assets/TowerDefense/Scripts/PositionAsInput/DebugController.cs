using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime + Vector3.right * Input.GetAxis("Horizontal")*Time.deltaTime;
    }
}
