using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsManagerBase : MonoBehaviour
{
    protected TowerType type;

    protected List<GameObject> enemiesList;
    protected List<Gun> gunsList;

    protected int numberOfEnemiesInRange;
    protected float currentDelay;
    protected int numberOfActiveGuns;
    protected int maxNumberOfGuns;

    [SerializeField]
    protected int upgradeRise;
    [SerializeField]
    protected int upgradeDamageCost;
    [SerializeField]
    protected int upgradeDamageIncreaseInPercent;
    [SerializeField]
    protected int upgradeNewGunCost;
    [SerializeField]
    protected int upgradeRangeCost;
    [SerializeField]
    protected int upgradeRangeIncreaseInPercent;
    [SerializeField]
    protected int maxDamage;
    [SerializeField]
    protected float maxRadius;
    [SerializeField]
    protected float shootingDelay = 3f;
    [SerializeField]
    protected float damage = 10f;
    [SerializeField]
    protected float speed = 8f;

    protected void deactivateGuns()
    {
        foreach (Gun g in gunsList)
        {
            g.gameObject.SetActive(false);
        }
    }

    public int GetNewGunCost()
    {
        return upgradeNewGunCost;
    }

    public int GetRangeIncreaseCost()
    {
        return upgradeRangeCost;
    }

    public int GetDamageIncreaseCost()
    {
        return upgradeDamageCost;
    }

    virtual protected IEnumerator shoot()
    {
        int t = 0;
        foreach (Gun g in gunsList)
        {
            if (g.gameObject.activeSelf)
                g.fire(enemiesList[t % numberOfEnemiesInRange], speed, damage, type);
            t++;
            yield return new WaitForSeconds(shootingDelay / numberOfActiveGuns / 2f);
        }
    }

    virtual public bool IsAddGunUpgradeAvailable()
    {
        return numberOfActiveGuns < maxNumberOfGuns;
    }

    virtual public bool IsFireDamageUpgradeAvailable()
    {
        return damage < maxDamage;
    }

    virtual public bool IsRangeUpgradeAvailable()
    {
        return this.GetComponent<SphereCollider>().radius < maxRadius;
    }

    virtual public void UpgradeFireDamage()
    {
        damage *= 1 + upgradeDamageIncreaseInPercent;
        upgradeDamageCost += upgradeRise;
    }

    virtual public void UpgradeAddGun()
    {
        gunsList[numberOfActiveGuns].gameObject.SetActive(true);
        numberOfActiveGuns++;
        upgradeNewGunCost += upgradeRise;
    }

    virtual public void UpgradeRange()
    {
        this.GetComponent<SphereCollider>().radius *= 1f + upgradeRangeIncreaseInPercent;
        upgradeRangeCost += upgradeRise;
    }
}
