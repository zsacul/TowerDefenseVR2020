using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsManager : MonoBehaviour
{
    List<GameObject> enemiesList;
    List<Gun> gunsList;

    int numberOfEnemiesInRange;
    int numberOfGuns;
    float currentDelay;

    [SerializeField]
    float shootingDelay = 3f;
    [SerializeField]
    float damage = 10f;
    [SerializeField]
    float speed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        gunsList = new List<Gun>(GetComponentsInChildren<Gun>());
        numberOfGuns = gunsList.Count;
        currentDelay = 0f;     
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if ( numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            foreach( Gun g in gunsList)
            {
                g.fire(enemiesList[0], speed, damage);
            }

            currentDelay = 0f;
        }
    }

}   