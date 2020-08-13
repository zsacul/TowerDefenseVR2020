using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDmgTextLifetime : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    private void Start()
    {
        StartCoroutine(reducing());
    }

    IEnumerator reducing()
    {
        while (text.color.a > 0f)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z - 0.01f);
            transform.position = new Vector3(transform.parent.position.x, transform.position.y + 0.05f, transform.parent.position.z);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
