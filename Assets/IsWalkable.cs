using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWalkable : MonoBehaviour
{
    GameObject isWlkble;
    // Start is called before the first frame update
    private void Awake()
    {
        isWlkble = GetComponentInChildren<UnwalkableChunk>().gameObject;
        isWlkble.SetActive(false);
    }

    public void ChangeIsWalkable(bool b)
    {
        if (b)
            isWlkble.SetActive(true);
        else
            isWlkble.SetActive(false);
    }
    // Update is called once per frame

}
