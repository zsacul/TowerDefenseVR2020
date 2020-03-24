using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTower : BaseTower
{
    [SerializeField]
    GameObject lightning;
    [SerializeField]
    GameObject ligtningMaker;
    // Start is called before the first frame update
    List<GameObject> lightningsList = new List<GameObject>();
    List<GameObject> hitedEnemiesList = new List<GameObject>();


    void Start()
    {
        type = TowerType.electricity;
        bulletPref = lightning;
        upgradeRise = 50;
        upgradeCost = 10;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        currentDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            Debug.Log("błyskawica");
            StartCoroutine(makeLightningChain(enemiesList[0]));
            currentDelay = 0f;
        }
    }


    IEnumerator  makeLightningChain(GameObject t)
    {
        GameObject target = t;
        GameObject startPoint = ligtningMaker;
        int numberOfHits = 1;
        int maxNumberOfHits = 4;
        float radius = 4f;
        
        while (target != null && numberOfHits < maxNumberOfHits)
        {
            createLightning(startPoint, target);
            target.GetComponent<EnemyHPManager>().ApplyDamage(new Bullet(target, 1, damage/numberOfHits, TowerType.electricity));
            startPoint = target;
            numberOfHits++;
            Debug.Log("szukamy nowego targetu");
            target = findNextEnemy(startPoint.transform.position, radius, hitedEnemiesList);
            Debug.Log(startPoint + " = " + target);  
        }

        yield return new WaitForSeconds(0.8f);
        deleteLightnings();
        hitedEnemiesList.Clear();
    }

    void createLightning(GameObject start, GameObject end)
    {
        Debug.Log("powstaje blyskawica");
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        instLightning.transform.parent = GetComponentInParent<triggerEnemiesCollisionList>().gameObject.transform;

        Debug.Log("prametry wchodza");
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = start;
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = end;

        Debug.Log("Powstala blyskawica z " + start.name + " do " + end.name);
        lightningsList.Add(instLightning);
        hitedEnemiesList.Add(end);

    }

    void deleteLightnings()
    {
        foreach (GameObject g in lightningsList)
            Destroy(g.gameObject);
    }

    GameObject findNextEnemy(Vector3 center, float radius, List<GameObject> hited)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject go = hitColliders[i].gameObject;
            if (go.tag == "Enemy" && !hited.Contains(go))
            {
                Debug.Log("Znaleziono nowego przeciwnika");
                return hitColliders[i].gameObject;
            }

            i++;
        }
        Debug.Log("nie znaleziono nowego qmpla");
        return null;
    }

    protected override void UpgradeAddGun()
    {
    }

}
