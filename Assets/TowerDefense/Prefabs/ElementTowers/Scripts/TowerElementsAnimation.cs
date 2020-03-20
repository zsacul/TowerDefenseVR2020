using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElementsAnimation : MonoBehaviour
{
    public List<ObjectRotator> Objects;
    public ObjectRotator ObjectRotator;
    public List<float> ObjectsHeights;
    public List<int> NumberOfObjectsOnHeight;
    public bool ActivateObjectsFalling = false;
    public float SpawnRateTime = 1;

    // Start is called before the first frame update
    private float spawnRateTime;
    void Start()
    {
        spawnRateTime = SpawnRateTime;
        if (!ActivateObjectsFalling)
        {
            for (int i = 0; i < ObjectsHeights.Count; i++)
            {
                for (int j = 0; j < NumberOfObjectsOnHeight[i]; j++)
                {
                    SpawnObjectOnHeight(ObjectsHeights[i]);
                }
            }
        }
        
    }

    private void Update()
    {
        SpawnFallingObject();
    }

    private void SpawnObjectOnHeight(float height)
    {
        ObjectRotator objectRotator = Instantiate<ObjectRotator>(ObjectRotator);
        ObjectRotator flyingObject = Instantiate<ObjectRotator>(ChooseRandomObject());

        objectRotator.transform.SetParent(transform);
        objectRotator.transform.position = new Vector3(transform.position.x, transform.position.y + UnityEngine.Random.Range(2f, 5.85f), transform.position.z);
        flyingObject.transform.SetParent(objectRotator.transform);
        flyingObject.transform.position = new Vector3(objectRotator.transform.position.x + 0.75f, objectRotator.transform.position.y, objectRotator.transform.position.z);
        
        if (ActivateObjectsFalling)
        {
            MakeObjectFall(objectRotator);
            objectRotator.ShouldFall = true;
            //Debug.LogError(gameObject.name);
        }
        else
        {
            objectRotator.RandomizeValues();
        }
    }

    private ObjectRotator ChooseRandomObject()
    {
        return Objects[UnityEngine.Random.Range(0, Objects.Count)];
    }

    private void MakeObjectFall(ObjectRotator objectRotator)
    {
        objectRotator.FallingSpeed = 0.2f;
        objectRotator.RotationSpeed = 50;
        objectRotator.Height = 6;
    }

    private void SpawnFallingObject()
    {
        if (ActivateObjectsFalling)
        {
            if (spawnRateTime < 0)
            {
                SpawnObjectOnHeight(ObjectsHeights[0]);
                spawnRateTime = SpawnRateTime;
            }
            else
            {
                spawnRateTime -= Time.deltaTime;
            }
        }
    }
}
