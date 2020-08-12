using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmgTextLifetime : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(reducing());
    }

    IEnumerator reducing()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z - 0.01f);
        if (transform.localScale.x < 0.1f)
            Destroy(gameObject);
        yield return new WaitForSeconds(0.01f);
    }
}
