using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDecorations : MonoBehaviour
{
    public List<GameObject> objectsOnTile;
    public Vector2 objectsAmountRange;
    public Vector2 randomSpaceLimitX;
    public Vector2 randomSpaceLimitY;
    public float upOffset = 0.03f;
    public bool randomizeAngleY = false;
    public bool randomizeAngleX = false;
    public bool randomizeAngleZ = false;
    public bool randomizeScale = false;
    public List<Vector2> objectChance = new List<Vector2> { new Vector2(0, 1) };

    void Start()
    {
        int stonesAmount = UnityEngine.Random.Range((int)objectsAmountRange.x, (int)objectsAmountRange.y);
        GameObject obj;
        for (int i = 0; i < stonesAmount; i++)
        {
            obj = ChooseRandomObject();
            if (obj != null)
            {
                //obj = Instantiate(ChooseRandomObject());
                obj = Instantiate(obj);
                obj.transform.SetParent(transform);
                if (randomizeAngleY)
                {
                    obj.transform.Rotate(new Vector3(obj.transform.rotation.x,
                                                         obj.transform.rotation.y + UnityEngine.Random.Range(0, 360),
                                                         obj.transform.rotation.z));
                }
                if (randomizeAngleX)
                {
                    obj.transform.Rotate(new Vector3(obj.transform.rotation.x + UnityEngine.Random.Range(3f, 9f),
                                                         obj.transform.rotation.y,
                                                         obj.transform.rotation.z));
                }
                if (randomizeAngleZ)
                {
                    obj.transform.Rotate(new Vector3(obj.transform.rotation.x,
                                                         obj.transform.rotation.y,
                                                         obj.transform.rotation.z + UnityEngine.Random.Range(3f, 9f)));
                }
                if (randomizeScale)
                {
                    float xz = UnityEngine.Random.Range(0.5f, 2f);
                    obj.transform.localScale = new Vector3(xz, xz, xz);
                }

                if (transform.rotation.y == 0)
                {
                    obj.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(randomSpaceLimitX.x, randomSpaceLimitX.y),
                                                         transform.position.y + upOffset,
                                                         transform.position.z + UnityEngine.Random.Range(randomSpaceLimitY.x, randomSpaceLimitY.y));
                }
                else
                {
                    obj.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(randomSpaceLimitY.x, randomSpaceLimitY.y),
                                                         transform.position.y + upOffset,
                                                         transform.position.z + UnityEngine.Random.Range(randomSpaceLimitX.x, randomSpaceLimitX.y));
                }
            }
            
        }
    }    

    private GameObject ChooseRandomObject()
    {
        float number = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < objectsOnTile.Count; i++)
        {
            if(objectChance[i].x < number && objectChance[i].y > number)
            {
                return objectsOnTile[i];
            }
        }
        return null;
        //return objectsOnTile[UnityEngine.Random.Range(0, objectsOnTile.Count)];
    }
}
