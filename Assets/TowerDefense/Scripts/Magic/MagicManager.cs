using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MagicManager : MonoBehaviour
{
    public Dictionary<string, bool> persistantState;
    public NodePos[] childNodes;
    public UnityEvent onActivate;
    private List<UINode> childrenUI;
    public UnityEvent onDeactivate;
    public Transform head;
    public GameObject targetMark;
    public SpellObject[] spells;
    private bool menuActive;
    private static MagicManager instance;
    private float charge;
    private IChargable current;
    public static bool GetPersistantState(string stateName)
    {
        if (!instance.persistantState.ContainsKey(stateName))
        {
            return false;
        }
        return instance.persistantState[stateName];
    }
    public static void SetPersistantState(string stateName, bool value)
    {
        if (instance.persistantState.ContainsKey(stateName))
        {
            instance.persistantState[stateName] = value;
            return;
        }
        instance.persistantState.Add(stateName, value);
    }
    private void Awake()
    {
        instance = this;
        persistantState = new Dictionary<string, bool>();
        HideMarker();
    }
    private void Start()
    {
        childrenUI = new List<UINode>();
    }
    public void SpawnChildNodes()
    {
        foreach (NodePos n in childNodes)
        {
            Vector3 pos = transform.position + n.relativePosition.x * head.right +
                                               n.relativePosition.y * head.up +
                                               n.relativePosition.z * head.forward;
            GameObject o = Instantiate(n.node, pos, head.rotation);
            childrenUI.Add(o.GetComponent<UINode>());
            o.GetComponent<UINode>().onSelect.AddListener(ChildSelected);
            o.GetComponent<UINode>().menuHead = head;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f)
        {
            if (!menuActive)
            {
                onActivate.Invoke();
            }
            menuActive = true;
        }
        else
        {
            if (menuActive)
            {
                onDeactivate.Invoke();
                Dispose();
            }
            menuActive = false;
        }
    }
    public static void Dispose()
    {
        foreach (UINode n in instance.childrenUI)
        {
            if (n != null)
            {
                n.Dispose();
            }
        }
        instance.childrenUI.Clear();
    }

    public void DisposeUnused(UINode keepAlive)
    {
        foreach (UINode n in childrenUI)
        {
            if (n != null && n != keepAlive)
            {
                n.Dispose();
            }
        }
        childrenUI.Clear();
        childrenUI.Add(keepAlive);
    }
    private void ChildSelected()
    {
        try
        {
            UINode keepAlive = childrenUI.ToArray().First<UINode>((UINode n) => n.selected);
            DisposeUnused(keepAlive);
        }
        catch (Exception e)
        {
            DisposeUnused(null);
        }
    }
    public static void Switch(bool state)
    {
        if (!state)
        {
            Dispose();
        }
        instance.enabled = state;

    }
    public static void SetMarker(Vector3 pos)
    {
        instance.targetMark.transform.position = pos;
        instance.targetMark.GetComponent<ParticleSystem>().Play();
    }
    public static void HideMarker()
    {
        instance.targetMark.GetComponent<ParticleSystem>().Stop();
    }
    public static void Cast(int index)
    {
        if (index > instance.spells.Length) return;
        instance.charge = 0;
        instance.spells[index].Cast(instance.transform.position, instance.transform.rotation, instance.charge, instance.transform);

    }
    public static void Release()
    {
        instance.current.Release();
    }
    public static void AddCharge(float charge)
    {
        instance.charge += charge;
        instance.current.SetCharge(instance.charge);
    }
}
