using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpiralAnimation : MonoBehaviour
{
    public List<ObjectRotator> Objects; // objects that can spawn
    public ObjectRotator ObjectRotator; // prefab to rotate spawned objects around pillar
    public float SpawnRateTime = 1; // how fast stones are spawn
    public float FallingSpeed;
    public float StartingHeight;
    public float RotationSpeed;

    public bool RandomizeAngle;

    // Start is called before the first frame update
    private float spawnRateTime;
    void Start()
    {
        spawnRateTime = SpawnRateTime;    
    }

    private void Update()
    {
        SpawnFallingObject();
    }

    private void SpawnObject()
    {
        ObjectRotator objectRotator = Instantiate<ObjectRotator>(ObjectRotator);
        ObjectRotator flyingObject = Instantiate<ObjectRotator>(ChooseRandomObject());
        //Debug.LogError("xd");
        Debug.LogError(transform.gameObject.name);
        objectRotator.transform.SetParent(transform);
        objectRotator.transform.position = new Vector3(transform.position.x, transform.position.y + StartingHeight, transform.position.z);
        flyingObject.transform.SetParent(objectRotator.transform);
        flyingObject.transform.position = new Vector3(objectRotator.transform.position.x + 0.75f, objectRotator.transform.position.y, objectRotator.transform.position.z);

        objectRotator.ShouldFall = true;
        objectRotator.FallingSpeed = FallingSpeed;
        objectRotator.RotationSpeed = RotationSpeed;
        objectRotator.Height = StartingHeight;

        if (RandomizeAngle) objectRotator.RandomizeAngle();
    }

    private ObjectRotator ChooseRandomObject()
    {
        return Objects[UnityEngine.Random.Range(0, Objects.Count)];
    }

    private void MakeObjectFall(ObjectRotator objectRotator)
    {
        objectRotator.FallingSpeed = 0.2f;
        
        objectRotator.Height = 6;
    }

    private void SpawnFallingObject()
    {        
        if (spawnRateTime < 0)
        {
            SpawnObject();
            spawnRateTime = SpawnRateTime;
        }
        else
        {
            spawnRateTime -= Time.deltaTime;
        }        
    }
}
