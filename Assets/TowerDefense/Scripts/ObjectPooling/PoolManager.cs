using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public bool profilerActive;
    public PoolManager Instance()
    {
        return instance;
    }
    public Dictionary<GameObject, Queue<GameObject>> pools;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        pools = new Dictionary<GameObject, Queue<GameObject>>();
        if(profilerActive)
        {
            InvokeRepeating("Profile", 0, 2);
        }
    }
    private void Profile()
    {
        Debug.Log($"PoolManager: used pools {pools.Count}");
        foreach(Queue<GameObject> q in pools.Values)
        {
            Debug.Log($"{q.Peek().name} count {q.Count}");
        }
    }
    static public GameObject PoolInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (instance.pools.ContainsKey(prefab) && instance.pools[prefab].Count > 0)
        {
            if(!instance.pools[prefab].Peek().activeSelf)
            {
                //There is object ready to use
                GameObject o = instance.pools[prefab].Dequeue();
                o.transform.position = position;
                o.transform.rotation = rotation;
                o.transform.parent = parent;
                o.SetActive(true);
                instance.pools[prefab].Enqueue(o);
                return o;
            }
            //all instances of this prefab are busy
            GameObject p = GameObject.Instantiate(prefab, position, rotation, parent);
            instance.pools[prefab].Enqueue(p);
            return p;
        }
        //prefab not registered yet
        if(instance.profilerActive)
        {
            Debug.Log("Register new Object");
        }
        GameObject l = GameObject.Instantiate(prefab, position, rotation, parent);
        instance.pools.Add(prefab, new Queue<GameObject>());
        instance.pools[prefab].Enqueue(l);
        return l;
    }
}
