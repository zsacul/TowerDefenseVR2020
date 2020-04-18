using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public int colortype;
    public float dupsko;
    public Animator GizmoAnimation;
    // Start is called before the first frame update
    public virtual void Remove()
    {
        //Debug.Log("Remove called");
        transform.gameObject.SetActive(false);

    }
    public virtual void Respawn()
    {
        //Debug.Log("Respawn called");
        transform.gameObject.SetActive(true);
    }

    public virtual void GrabEvent(float input)
    {
        //Debug.Log($"Dupa {input} Animator {GizmoAnimation.name} {GizmoAnimation.GetFloat(1)}");
        GizmoAnimation.SetFloat("GripFloat", input);
    }

    public virtual void PointEvent(float input)
    {
        //Debug.Log($"Dupa {input} Animator {GizmoAnimation.name} {GizmoAnimation.GetFloat(1)}");
        GizmoAnimation.SetFloat("PointFloat", input);
    }

    public virtual void ThumbEvent(bool input)
    {
        Debug.Log($"ThmbEvent {input}");
        if (input == true)
            GizmoAnimation.SetFloat("ThumbFloat", 0.99f);
        else
            GizmoAnimation.SetFloat("ThumbFloat", 0.01f);
    }

    public virtual void Initialize()
    {
        //Debug.Log("Initialize called");
        GameObject Cylinder = transform.GetChild(1).gameObject;
        //GizmoAnimation = GetComponent<Animator>();
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
