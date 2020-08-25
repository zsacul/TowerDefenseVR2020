using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    public static Endgame Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetScene()
    {
        StartCoroutine(WaitAndRestart());
    } 

    IEnumerator WaitAndRestart()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
