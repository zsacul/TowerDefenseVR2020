using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wand : MonoBehaviour
{
    public Spell[] spells;
    private List<Vector2Int> currentPath;
    private Vector2Int startPos;
    private float charge;

    public UnityEvent onStartCast; 
    public UnityEvent onCastSucces; 
    public UnityEvent onCastFailure;
    private ControllerPosition instance;
    private bool casting;
    IChargable current;
    private void Start()
    {
        instance = ControllerPosition.Instance(false);
        instance.inputChanged.AddListener(InputPosChanged);
        instance.inFrontEnter.AddListener(InFrontEnter);
        instance.inFrontExit.AddListener(InFrontExit);
        currentPath = new List<Vector2Int>();
    }
    private void Update()
    {
        if(!casting && (Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f || Input.GetKey(KeyCode.LeftControl)))
        {
            CastStart();
        }
        if(casting && (Input.GetAxis("VRTK_Axis10_RightTrigger") < 0.1f && !Input.GetKey(KeyCode.LeftControl)))
        {
            CastEnd();
        }
    }
    void InputPosChanged(Vector2Int pos)
    {
        if(Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f || Input.GetKey(KeyCode.LeftControl))
        {
            CastUpdate(pos);
        }
        else
        {
            //end or passive update
            if(!casting)
            {
                startPos = pos;
            }
        }
    }
    void InFrontEnter()
    {
        
    }
    void InFrontExit()
    {

    }
    private void OnDestroy()
    {
        ControllerPosition instance = ControllerPosition.Instance(false);
        instance.inputChanged.RemoveListener(InputPosChanged);
        instance.inFrontEnter.RemoveListener(InFrontEnter);
        instance.inFrontExit.RemoveListener(InFrontExit);
    }
    private void CastStart()
    {
        instance.StartMove();
        currentPath.Clear();
        currentPath.Add(Vector2Int.zero);
        onStartCast.Invoke();
        casting = true;
    }
    private void CastUpdate(Vector2Int pos)
    {
        if (current != null) return;
        currentPath.Add(pos);
        Vector2Int[] sequence = currentPath.ToArray();
        for (int i = 0; i < spells.Length; i++)
        {
            if (SequenceMatch(spells[i].castSequence, sequence))
            {
                current = spells[i].Cast(transform.position, transform.rotation, charge, transform);
                onCastSucces.Invoke();
                return;
            }
        }
    }
    private void CastEnd()
    {
        casting = false;
        if(current != null)
        {
            current.Release();
            current = null;
        }
        else
        {
            onCastFailure.Invoke();
        }
    }
    private bool SequenceMatch(Vector2Int[] template, Vector2Int[] sequence)
    {
        int j = 0;//iterator on template
        Vector2Int previous = template[0];
        for(int i = 0; i < sequence.Length && j < template.Length; i++)
        {
            //good match
            if(Vector2Int.Distance(sequence[i], template[j])<2)
            {
                previous = template[j];
                j++;
                continue;
            }
            //neutral match
            if(Vector2Int.Distance(sequence[i], previous) < 2)
            {
                continue;
            }
            //negative match
            return false;
        }
        if(j < template.Length - 1)
        {
            return false;
        }
        return true;
    }
}
