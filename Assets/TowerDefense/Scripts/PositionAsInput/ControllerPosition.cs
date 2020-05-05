using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerPosition : MonoBehaviour
{
    public Transform controller;
    public Transform head;
    public bool isLeft;
    [Tooltip("z used only for debug display")]
    public Vector3 offset;
    public Vector2 areaSize;
    [Tooltip("resolution of input grid")]
    public Vector2Int resolution;
    public UnityEvent<Vector2Int> inputChanged;
    public UnityEvent inFrontEnter;
    public UnityEvent inFrontExit;
    private bool inFront;
    private static ControllerPosition left;
    private static ControllerPosition right;
    public Vector2Int lastPos;
    // Start is called before the first frame update
    void Awake()
    {
        inputChanged = new UnityEvent2Int();
        if(isLeft)
        {
            left = this;
        }
        else
        {
            right = this;
        }
    }
    public static ControllerPosition Instance(bool isLeft)
    {
        return isLeft ? left : right;
    }
    private void FixedUpdate()
    {
        ControllerCardinal();
        InFront();
    }
    private void InFront()
    {
        Vector3 heading = controller.position - head.position;
        float dot = Vector3.Dot(heading, head.forward);
        bool newInFront = dot > 0 ? true : false;
        if(newInFront != inFront)
        {
            inFront = newInFront;
            if(inFront)
            {
                inFrontEnter.Invoke();
            }
            else
            {
                inFrontExit.Invoke();
            }
        }
    }
    void ControllerCardinal()
    {
        Vector3 relPos = head.InverseTransformPoint(controller.position);
        float x = relPos.x;
        float y = relPos.y;
        Vector2Int pos = PosInput(x, y);
        if(pos.x != lastPos.x || pos.y != lastPos.y)
        {
            inputChanged.Invoke(pos);
            lastPos = pos;
        }
    }
    Vector2Int PosInput(float x, float y)
    {
        float xMin = offset.x;
        float xMax = offset.x + areaSize.x;
        float yMin = offset.y;
        float yMax = offset.y + areaSize.y;
        float xDelta = (xMax - xMin) / resolution.x;
        float yDelta = (yMax - yMin) / resolution.y;
        x -= xMin;
        y -= yMin;
        int xI = (int)(x / xDelta);
        int yI = (int)(y / yDelta);
        return new Vector2Int(xI, yI);
    }
}
