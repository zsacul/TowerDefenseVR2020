using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    protected TowerType type;

    protected List<GameObject> enemiesList;
    protected List<Gun> gunsList;

    protected int numberOfEnemiesInRange;
    protected int numberOfActiveGuns;
    protected int maxNumberOfGuns;
    protected int lvl;
    protected int maxlvl;
    protected float currentDelay;
    protected GameObject bulletPref;

    [SerializeField]
    protected int upgradeRise;
    [SerializeField]
    protected int upgradeCost;
    [SerializeField]
    protected int upgradeDamageIncreaseInPercent;
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

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    public int Getlvl()
    {
        return lvl;
    }

    public void Upgrade()
    {
        if (lvl < maxlvl)
        {
            UpgradeFireDamage();
            UpgradeRange();
            UpgradeAddGun();
            lvl++;
            upgradeCost *= upgradeRise;
        }
    }

    protected void deactivateGuns()
    {
        foreach (Gun g in gunsList)
        {
            g.gameObject.SetActive(false);
        }
    }

    virtual protected void setBulletTypeInGuns()
    {
        foreach (Gun g in gunsList)
            g.SetBullet(bulletPref);
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
    
    virtual protected void UpgradeFireDamage()
    {
        damage *= 1 + upgradeDamageIncreaseInPercent;
    }

    virtual protected void UpgradeAddGun()
    {
        gunsList[numberOfActiveGuns].gameObject.SetActive(true);
        numberOfActiveGuns++;
    }

    virtual protected void UpgradeRange()
    {
        this.GetComponent<SphereCollider>().radius *= 1f + upgradeRangeIncreaseInPercent;
    }
}
