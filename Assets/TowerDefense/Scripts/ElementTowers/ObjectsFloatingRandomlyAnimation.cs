using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsFloatingRandomlyAnimation : MonoBehaviour
{
    public List<ObjectRotator> Objects;
    public ObjectRotator ObjectRotator;
    public int NumberOfObjects;

    void Start()
    {
        for (int i = 0; i < NumberOfObjects; i++)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        ObjectRotator objectRotator = Instantiate<ObjectRotator>(ObjectRotator);
        ObjectRotator flyingObject = Instantiate<ObjectRotator>(ChooseRandomObject());

        objectRotator.transform.SetParent(transform);
        objectRotator.transform.position = new Vector3(transform.position.x, transform.position.y + UnityEngine.Random.Range(2f, 5.85f), transform.position.z);
        flyingObject.transform.SetParent(objectRotator.transform);
        flyingObject.transform.position = new Vector3(objectRotator.transform.position.x + 0.75f, objectRotator.transform.position.y, objectRotator.transform.position.z);

        objectRotator.RandomizeValues();
    }

    private ObjectRotator ChooseRandomObject()
    {
        return Objects[UnityEngine.Random.Range(0, Objects.Count)];
    }




}
