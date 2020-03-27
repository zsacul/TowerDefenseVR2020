using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float MinFloatHeight;
    public float MaxFloatHeight;
    public float FloatSpeed;

    private int floatDirection = 1;
    private float deltaHeight = 0;
    private float originalHeight;

    private void Start()
    {
        originalHeight = transform.position.y;
    }
    
    void FixedUpdate()
    {
        if(deltaHeight > MaxFloatHeight || deltaHeight < MinFloatHeight)
        {
            floatDirection = -floatDirection;
            deltaHeight = floatDirection == 1 ? MinFloatHeight : MaxFloatHeight;
        }

        deltaHeight += floatDirection * FloatSpeed;
        transform.position = new Vector3(transform.position.x, originalHeight + deltaHeight, transform.position.z);
    }
}
