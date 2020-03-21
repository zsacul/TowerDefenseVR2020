using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 RotationSpeeds;
    public Vector3 StartAngles;
    public bool ShouldFall = false;


    private int rotationDirection = 1;
    private float height;
    private float destroyHeightDown = 0;
    private float destroyHeightUp = 10;
    private float fallingSpeed;

    public void SetFall(Vector2 FallRange, float FallingSpeed)
    {
        fallingSpeed = FallingSpeed;
        if (fallingSpeed < 0)
        {
            height = FallRange.x;
            destroyHeightUp = FallRange.y;
        }
        else
        {
            height = FallRange.y;
            destroyHeightDown = FallRange.x;
        }
        transform.position = new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y + height, transform.parent.transform.position.z);
    }

    public void RandomizeAngle()
    {
        StartAngles.y = UnityEngine.Random.Range(0, 360f);
    }

    public void RandomizeValues()
    {
        RotationSpeeds.y += UnityEngine.Random.Range(0, 5);
        StartAngles.y = UnityEngine.Random.Range(0, 360f);
        rotationDirection = UnityEngine.Random.Range(0, 2);
        rotationDirection = rotationDirection == 0 ? -1 : 1;
    }



    void Update()
    {
        if (ShouldFall)
        {
            height -= fallingSpeed * Time.deltaTime;
        }

        StartAngles.x += RotationSpeeds.x * Time.deltaTime * rotationDirection;
        StartAngles.y += RotationSpeeds.y * Time.deltaTime * rotationDirection;
        StartAngles.z += RotationSpeeds.z * Time.deltaTime * rotationDirection;

        transform.localRotation = Quaternion.Euler(new Vector3(StartAngles.x, StartAngles.y, StartAngles.z));
        transform.position = new Vector3(transform.position.x, ShouldFall ? height : transform.position.y, transform.position.z);

        if (ShouldFall && height < destroyHeightDown || height > destroyHeightUp)
        {
            Destroy(this.gameObject.GetComponentInChildren<ObjectRotator>().gameObject);
            Destroy(this);
        }
    }
}
