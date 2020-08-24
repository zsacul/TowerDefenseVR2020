using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    public static Endgame Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetScene()
    {
        Debug.Log("ResetScene() not implemented");
    } 
}
