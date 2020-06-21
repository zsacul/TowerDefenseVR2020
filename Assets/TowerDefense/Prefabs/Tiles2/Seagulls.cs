using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagulls : MonoBehaviour
{
    private Animator animator;
    private int RandomStartTime;
    // Start is called before the first frame update
    void Start()
    {
        RandomStartTime = Random.Range(0, 160);
        animator = GetComponent<Animator>();
        animator.Play("Flying", -1, RandomStartTime * 1.0f / 160);
    }

    
}
