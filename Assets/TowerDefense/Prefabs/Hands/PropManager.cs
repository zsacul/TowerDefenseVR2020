using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public int colortype;
    // Start is called before the first frame update
    public void Remove()
    {
        Debug.Log("Remove called");
        transform.gameObject.SetActive(false);

    }
    public void Respawn()
    {
        Debug.Log("Respawn called");
        transform.gameObject.SetActive(true);
    }

    public void Initialize()
    {
        Debug.Log("Initialize called");
        GameObject Cylinder = transform.GetChild(1).gameObject;
        var HandRenderer = Cylinder.GetComponent<Renderer>();
        if (colortype == 0)
            HandRenderer.material.SetColor("_Color", Color.red);
        if (colortype == 1)
            HandRenderer.material.SetColor("_Color", Color.green);
        if (colortype == 2)
            HandRenderer.material.SetColor("_Color", Color.blue);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
