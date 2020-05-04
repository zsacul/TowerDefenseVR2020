using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextHandler : MonoBehaviour
{
    public Text mon;
    // Update is called once per frame
    void Update()
    {
        Vector3 monPos = Camera.main.WorldToScreenPoint(this.transform.position);
        mon.transform.position = monPos;
    }
}
