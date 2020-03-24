using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpiralAnimation : MonoBehaviour
{
    public List<GameObject> Objects; // objects that can spawn
    public ObjectRotator ObjectRotator; // prefab to rotate spawned objects around pillar
    public float SpawnRateTime = 1; // how fast stones are spawn
    public float FallingSpeed; // falling speed below 0 makes object fly up
    public Vector2 FallRange;  // height between objects exist
    public float RotationSpeed; // how fast objects rotate around center
    public bool RandomizeAngle; //should start y angle be randomized
    public float RotationRadius; // how far rotating object is from pillar
    public bool ShouldSpawn = true;

    void Start()
    {
        InvokeRepeating("SpawnFallingObject", 0.0f, SpawnRateTime);
    }

    private void SpawnObject()
    {
        ObjectRotator objectRotator = Instantiate(ObjectRotator);
        GameObject flyingObject = Instantiate(ChooseRandomObject());
        objectRotator.transform.SetParent(transform);
        flyingObject.transform.SetParent(objectRotator.gameObject.transform);
        flyingObject.transform.position = new Vector3(objectRotator.transform.position.x + RotationRadius, objectRotator.transform.position.y, objectRotator.transform.position.z);

        objectRotator.ShouldFall = true;
        objectRotator.RotationSpeeds.y = RotationSpeed;
        objectRotator.SetFall(FallRange, FallingSpeed);

        if (RandomizeAngle) objectRotator.RandomizeAngle();
    }

    private GameObject ChooseRandomObject()
    {
        return Objects[UnityEngine.Random.Range(0, Objects.Count)];
    }

    private void SpawnFallingObject()
    {
        if(ShouldSpawn)
        {
            SpawnObject();
        }
    }
}
