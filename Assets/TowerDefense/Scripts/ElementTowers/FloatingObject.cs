using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // Start is called before the first frame update
    public float MinFloatHeight;
    public float MaxFloatHeight;
    public float FloatSpeed;

    private int floatDirection = 1;
    float deltaHeight = 0;
    float originalHeight;

    private void Start()
    {
        originalHeight = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        if(deltaHeight > MaxFloatHeight || deltaHeight < MinFloatHeight)
        {
            floatDirection = -floatDirection;
            deltaHeight = floatDirection == 1 ? MinFloatHeight : MaxFloatHeight;
        }
        //Debug.LogError(floatDirection + " " + deltaHeight);
        deltaHeight += floatDirection * FloatSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, originalHeight + deltaHeight, transform.position.z);
    }
}
