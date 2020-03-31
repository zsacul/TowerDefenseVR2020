using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElectricTower : BaseTower
{
    [SerializeField]
    GameObject lightning;
    [SerializeField]
    GameObject ligtningMaker;
    [SerializeField]
    GameObject EnemiesTarget;
    // Start is called before the first frame update
    List<GameObject> lightningsList = new List<GameObject>();
    List<GameObject> hitedEnemiesList = new List<GameObject>();

    float speedOfShaking = 40.0f; //how fast it shakes
    float amountOfShaking = 10.0f; //how much it shakes
 


    void Start()
    {
        EnemiesTarget = FindObjectOfType<EndpointManager>().gameObject;
        type = ElementType.electricity;

        sound = GetComponent<BaseSoundAttachment>();

        bulletPref = lightning;
        upgradeRise = 50;
        upgradeCost = 10;
        enemiesList = GetComponent<triggerEnemiesCollisionList>().getCollidersList();
        currentDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        hitedEnemiesList.RemoveAll(item => item == null || item.GetComponent<Collider>() == null);
        currentDelay += Time.deltaTime;
        numberOfEnemiesInRange = enemiesList.Count;
        if (numberOfEnemiesInRange > 0 && currentDelay >= shootingDelay)
        {
            Debug.Log("błyskawica");
            StartCoroutine(makeLightningChain(enemiesList[0]));
            currentDelay = 0f;
        }
        if(hitedEnemiesList.Count > 0)
            dealHitedEnemies();
    }

    void dealHitedEnemies(){
        foreach (GameObject e in hitedEnemiesList){
            Vector3 rot = e.transform.rotation.eulerAngles;
            rot.y += Mathf.Sin(Time.time * speedOfShaking) * amountOfShaking;
            e.transform.rotation = Quaternion.Euler(rot);
        }
    }


    IEnumerator  makeLightningChain(GameObject t)
    {
        Debug.Log("NOWA SEKWENCJA");
        if (sound != null)
            sound.Play();
        GameObject target = t;
        GameObject startPoint = ligtningMaker;
        int numberOfHits = 1;
        int maxNumberOfHits = 4;
        float radius = 4f;

        while (target != null && numberOfHits < maxNumberOfHits)
        {
            target.GetComponent<NavMeshAgent>().destination = target.transform.position;
            createLightning(startPoint, target);
            startPoint = target.GetComponent<EnemyHPManager>().GetTargetPoint();
            numberOfHits++;
            Debug.Log("szukamy nowego targetu");
            target = findNextEnemy(startPoint.transform.position, radius, hitedEnemiesList);
            Debug.Log(startPoint + " = " + target);  
        }

        yield return new WaitForSeconds(0.8f);
        deleteLightnings();
        deleteHitedEnemyList();
    }

   void deleteHitedEnemyList(){
        int i = 1;
        foreach( GameObject e in hitedEnemiesList){
            e.GetComponent<NavMeshAgent>().destination = EnemiesTarget.transform.position;
            e.GetComponent<EnemyHPManager>().ApplyDamage(new Bullet(e, 1, damage / i, ElementType.electricity));
            i++;
        }
        hitedEnemiesList.Clear();
    }

    void createLightning(GameObject start, GameObject end)
    {
        Debug.Log("powstaje blyskawica");
        GameObject instLightning = Instantiate(lightning, transform.position, Quaternion.identity) as GameObject;
        instLightning.transform.parent = GetComponentInParent<triggerEnemiesCollisionList>().gameObject.transform;

        Debug.Log("prametry wchodza");
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = start;
        instLightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = end.GetComponent<EnemyHPManager>().GetTargetPoint();

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
