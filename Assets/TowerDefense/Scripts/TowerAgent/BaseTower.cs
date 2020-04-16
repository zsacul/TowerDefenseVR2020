using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    protected ElementType type;

    protected List<GameObject> enemiesList;
    protected List<Gun> gunsList;

    protected int numberOfEnemiesInRange;
    protected int numberOfActiveGuns;
    protected int maxNumberOfGuns;
    protected int lvl = 1;
    protected int maxlvl = 4;
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
    protected float shootingDelay;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int specialEffectDuration;
    [SerializeField]
    protected int specialEffectDmg;

    protected BaseSoundAttachment sound;

    public int GetUpgradeCost()
    {
        return upgradeCost;
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
            if (g.gameObject.activeSelf && enemiesList.Count > 0)
            {
                g.fire(enemiesList[t % numberOfEnemiesInRange], speed, damage, type, specialEffectDuration, specialEffectDmg);
                if (sound != null)
                    sound.Play();
            }
            t++;
            yield return new WaitForSeconds(shootingDelay / numberOfActiveGuns / 2f);
        }
    }
    
    virtual public void UpgradeFireDamage()
    {
        damage *= 1f + ((float)upgradeDamageIncreaseInPercent)/100f;
    }

    virtual public void UpgradeAddGun()
    {
        gunsList[numberOfActiveGuns].gameObject.SetActive(true);
        numberOfActiveGuns++;
    }

    virtual public void UpgradeRange()
    {
        GetComponent<SphereCollider>().radius *= 1f + ((float)upgradeRangeIncreaseInPercent)/100f;
    }
}
