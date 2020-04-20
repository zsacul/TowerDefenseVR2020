using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    [SerializeField]
    Material normalMaterial;
    [SerializeField]
    Material visibilityMaterial;
    [SerializeField]
    float changingVisibilitySpeed = 0.2f;
    [SerializeField]
    float appearingDelay = 0f;

    private bool appear = false;
    private bool disappear = false;
    private float invisiblePart = 1;
    private MeshRenderer r;
    private SkinnedMeshRenderer sr;

    private void Start()
    {
        r = GetComponent<MeshRenderer>();
        sr = GetComponent<SkinnedMeshRenderer>();
        if (r == null)
        {
            sr.material = visibilityMaterial;
        }
        else
        {
            r.material = visibilityMaterial;
        }
        Invoke("StartAppearing", appearingDelay);
        visibilityMaterial.SetFloat("Vector1_8D124B45", invisiblePart);
    }

    void Update()
    {
        if(appear)
        {
            invisiblePart -= changingVisibilitySpeed * Time.deltaTime;
            visibilityMaterial.SetFloat("Vector1_8D124B45", invisiblePart);
            if (invisiblePart <= 0)
            {
                if (r == null)
                {
                    sr.material = normalMaterial;
                }
                else
                {
                    r.material = normalMaterial;
                }
                appear = false;
            }
        }
        if(disappear)
        {
            invisiblePart += changingVisibilitySpeed * Time.deltaTime;
            visibilityMaterial.SetFloat("Vector1_8D124B45", invisiblePart);
            if(invisiblePart >= 1)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartDisappearing()
    {
        if (!appear)
        {
            if (r == null)
            {
                sr.material = visibilityMaterial;
            }
            else
            {
                r.material = visibilityMaterial;
            }
            disappear = true;
        }
    }

    public void StartAppearing()
    {
        appear = true;
    }
}
