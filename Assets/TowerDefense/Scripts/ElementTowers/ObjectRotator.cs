using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 RotationSpeeds;
    public Vector3 StartAngles;
    public Vector3 RepeatAngles;
    public bool ShouldFall = false;

    private Vector3 Angles;
    private int rotationDirection = 1;
    private float height;
    private float destroyHeightDown = 0;
    private float destroyHeightUp = 10;
    private float fallingSpeed;
    public bool randomYAngle = false;

    public void SetFall(Vector2 FallRange, float FallingSpeed)
    {
        fallingSpeed = FallingSpeed;
        if (fallingSpeed < 0)
        {
            height = FallRange.x;
            destroyHeightUp = FallRange.y;
            destroyHeightDown = FallRange.x;
        }
        else
        {
            height = FallRange.y;
            destroyHeightDown = FallRange.x;
            destroyHeightUp = FallRange.y;
        }
        transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y + height, transform.parent.transform.position.z);
    }

    public void RandomizeAngle()
    {
        StartAngles.y = UnityEngine.Random.Range(0, 360f);
        randomYAngle = true;
    }

    public void RandomizeValues()
    {
        RotationSpeeds.y += UnityEngine.Random.Range(0, 0.2f);
        StartAngles.y = UnityEngine.Random.Range(0, 360f);
        rotationDirection = UnityEngine.Random.Range(0, 2);
        rotationDirection = rotationDirection == 0 ? -1 : 1;
    }

    void FixedUpdate()
    {
        if (ShouldFall)
        {
            height -= fallingSpeed;
        }

        StartAngles.x += RotationSpeeds.x * rotationDirection;
        StartAngles.y += RotationSpeeds.y * rotationDirection;
        StartAngles.z += RotationSpeeds.z * rotationDirection;

        transform.localRotation = Quaternion.Euler(new Vector3(StartAngles.x, StartAngles.y, StartAngles.z));
        transform.position = new Vector3(transform.position.x, ShouldFall ? height : transform.position.y, transform.position.z);

        if (ShouldFall && height < destroyHeightDown || height > destroyHeightUp)
        {
            if(fallingSpeed < 0)
            {
                height = destroyHeightDown;
            }
            else
            {
                height = destroyHeightUp;
            }
            StartAngles = RepeatAngles;
            if (randomYAngle) RandomizeAngle();
            ObjectsSpiralAnimation anim = GetComponentInParent<ObjectsSpiralAnimation>();
            if (anim != null)
            {
                anim.ShouldSpawn = false;
            }
        }
    }
}
