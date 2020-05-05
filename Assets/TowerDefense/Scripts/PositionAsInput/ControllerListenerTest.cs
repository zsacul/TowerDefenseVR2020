using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerListenerTest : MonoBehaviour
{
    public bool isLeft;
    private void Start()
    {
        ControllerPosition instance = ControllerPosition.Instance(isLeft);
        instance.inputChanged.AddListener(InputPosChanged);
        instance.inFrontEnter.AddListener(InFrontEnter);
        instance.inFrontExit.AddListener(InFrontExit);
    }
    void InputPosChanged(Vector2Int pos)
    {
        Debug.Log(pos);
    }
    void InFrontEnter()
    {
        Debug.Log("enter");
    }
    void InFrontExit()
    {
        Debug.Log("exit");
    }
    private void OnDestroy()
    {
        ControllerPosition instance = ControllerPosition.Instance(isLeft);
        instance.inputChanged.RemoveListener(InputPosChanged);
        instance.inFrontEnter.RemoveListener(InFrontEnter);
        instance.inFrontExit.RemoveListener(InFrontExit);
    }
}
