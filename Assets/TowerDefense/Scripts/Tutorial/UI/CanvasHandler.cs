using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    public Canvas canvas;
    public Text text;
    public Vector3 canvasRotation;
    public Vector3 canvasScale;
    public GameEvent instantiateEvent;
    public bool shouldRaise;

    private Canvas canvasInstance;
    private Text textInstance;

    void Start()
    {
        canvasInstance = Instantiate(canvas);
        textInstance = Instantiate(text);
        canvasInstance.transform.position = transform.position;
        canvasInstance.transform.rotation = Quaternion.Euler(canvasRotation);
        canvasInstance.transform.localScale = canvasScale;
        textInstance.gameObject.transform.SetParent(canvasInstance.transform, false);
        if (shouldRaise)
        {
            instantiateEvent.Raise();
        }
    }
}
