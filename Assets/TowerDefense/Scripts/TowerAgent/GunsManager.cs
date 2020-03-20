using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsManager : MonoBehaviour
{
    List<GameObject> enemiesList;
    List<Gun> gunsList;

    int numberOfEnemiesInRange;
    float currentDelay;
    string type;
    int numberOfActiveGuns;
    int maxNumberOfGuns;
    int upgradeDamageCost;
    int upgradeNewGunCost;

    [SerializeField]
    float shootingDelay = 3f;
    [SerializeField]
    float damage = 10f;
    [SerializeField]
    float speed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        upgradeDamageCost = 10;
        upgradeNewGunCost = 10;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        gunsList = new List<Gun>(GetComponentsInChildren<Gun>());
        maxNumberOfGuns = gunsList.Count;
        currentDelay = 0f;
        numberOfActiveGuns = 1;
        type = "default";

        deactivateGuns();
        gunsList[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if ( numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            StartCoroutine(shoot());
            currentDelay = 0f;
            UpgradeAddGun();
        }
    }

    IEnumerator shoot()
    {
        int t = 0;
        foreach (Gun g in gunsList)
        {
            if (g.gameObject.activeSelf)
                g.fire(enemiesList[t % numberOfEnemiesInRange], speed, damage, type);
            t++;
            yield return new WaitForSeconds(shootingDelay/numberOfActiveGuns/2f);
        }
    }

    public void SetType(string str)
    {
        switch (str)
        {
            case "stone":
                type = str;
                break;
            case "fire":
                type = str;
                break;
            case "ice":
                type = str;
                break;
            case "bolt":
                type = str;
                break;
            default:
                break;
        }
        if (type == "defoult")
            Debug.Log("ZŁY RODZAJ WIEŻY!!!");
    }

    public void UpgradeFireDamage()
    {
        if (checkMoney( upgradeDamageCost))
        {
            damage *= 1.5f;
            upgradeDamageCost *= 2;
        }
    }

    public void UpgradeAddGun()
    {
        if (numberOfActiveGuns < maxNumberOfGuns)
            if (checkMoney(upgradeNewGunCost))
            {
                gunsList[numberOfActiveGuns].gameObject.SetActive(true);
                numberOfActiveGuns++;
                upgradeNewGunCost *= 2;
            }
    }

    bool checkMoney( int cost)
    {
        return true;
    }

    void deactivateGuns()
    {
        foreach( Gun g in gunsList)
        {
            g.gameObject.SetActive(false);
        }
    }
}   