using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    public GameObject chunk;
    [SerializeField]
    private int elementIndex;

    void OnTriggerEnter(Collider other)
    {
        chunk.GetComponent<Chunk>().UpgradeTower(elementIndex);
    }
}
