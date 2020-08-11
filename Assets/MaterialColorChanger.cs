using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorChanger : MonoBehaviour
{
    public Color BaseColor, FreezeColor;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    [SerializeField]
    Material BoximonDefaultMaterial;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _propBlock.SetColor("_BaseColor", BoximonDefaultMaterial.color);
        Debug.Log(_propBlock.GetColor("_BaseColor"));
        _renderer = GetComponent<Renderer>();
        _renderer.SetPropertyBlock(_propBlock);
    }

    public IEnumerator SetFreezColor()
    {
        while (_propBlock.GetColor("_BaseColor").r > 0f)
        {
            _renderer.GetPropertyBlock(_propBlock);
            // Assign our new value.
            _propBlock.SetColor("_BaseColor", new Color(_propBlock.GetColor("_BaseColor").r - 0.01f, _propBlock.GetColor("_BaseColor").g, _propBlock.GetColor("_BaseColor").b));
            // Apply the edited values to the renderer.
            if (_propBlock.GetColor("_BaseColor").r < 0)
                break;
            Debug.Log(_propBlock.GetColor("_BaseColor"));
            _renderer.SetPropertyBlock(_propBlock);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator SetNormalColor()
    {
        while (_propBlock.GetColor("_BaseColor").r < 1f)
        {
            _renderer.GetPropertyBlock(_propBlock);
            // Assign our new value.
            _propBlock.SetColor("_BaseColor", new Color(_propBlock.GetColor("_BaseColor").r + 0.01f, _propBlock.GetColor("_BaseColor").g, _propBlock.GetColor("_BaseColor").b));
            // Apply the edited values to the renderer.
            if (_propBlock.GetColor("_BaseColor").r < 0)
                break;
            Debug.Log(_propBlock.GetColor("_BaseColor"));
            _renderer.SetPropertyBlock(_propBlock);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
