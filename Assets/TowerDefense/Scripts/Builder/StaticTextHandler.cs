using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticTextHandler : MonoBehaviour
{
    public Text text;
    void Update()
    {
        Vector3 monPos = Camera.main.WorldToScreenPoint(this.transform.position);
        text.transform.position = monPos;
    }
}
