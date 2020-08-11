using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseTower : MonoBehaviour
{
    protected ElementType type;
    protected SpecialEffect SE = SpecialEffect.none;
     
    protected List<GameObject> enemiesList;
    protected List<Gun> gunsList;

    protected int numberOfEnemiesInRange;
    protected int numberOfActiveGuns = 1;
    protected int maxNumberOfGuns;
    protected int lvl = 1;
    protected int maxlvl = 4;
    protected float currentDelay;
    protected GameObject bulletPref;

    [SerializeField]
    protected int upgradeRiseInPercent;
    [SerializeField]
    protected int upgradeCostDMG;
    [SerializeField]
    protected int upgradeCostRange;
    [SerializeField]
    protected int upgradeCostDelay;
    [SerializeField]
    protected int upgradeCostAddGun;
    [SerializeField]
    protected int upgradeDamageIncreaseInPercent;
    [SerializeField]
    protected int upgradeRangeIncreaseInPercent;
    [SerializeField]
    protected int upgradeDelayIncreaseInPercent;
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

    public UnityEvent shootEvent, updateRange, updateDMG, updateDelay, updateAddGun;
    public int UpgradeCostDMG    => upgradeCostDMG;
    public int UpgradeCostRange  => upgradeCostRange;
    public int UpgradeCostDelay  => upgradeCostDelay;
    public int UpgradeCostAddGun => upgradeCostAddGun;

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
        shootEvent.Invoke();
        int t = 0;
        foreach (Gun g in gunsList)
        {
            if (g.gameObject.activeSelf && enemiesList.Count > 0)
            {
                g.fire(enemiesList[t % numberOfEnemiesInRange].GetComponentInChildren<EnemyTargetPoint>().gameObject, speed, damage, type, specialEffectDuration, specialEffectDmg, SE);
                if (sound != null)
                    sound.Play();
            }
            t++;
            yield return new WaitForSeconds(shootingDelay / numberOfActiveGuns / 2f);
        }
    }

    virtual protected void updateSE(SpecialEffect se)
    {
        SE = se;
    }

    virtual public (float, int) DamageStatInfo()
    {
        return (damage, upgradeDamageIncreaseInPercent);
    }

    virtual public (float, int) DelayStatInfo()
    {
        return (shootingDelay, upgradeDelayIncreaseInPercent);
    }

    virtual public (float, int) RangeStatInfo()
    {
        return (GetComponent<SphereCollider>().radius, upgradeRangeIncreaseInPercent);
    }

    virtual public void UpgradeFireDamage()
    {
        updateDMG.Invoke();
        damage *= 1f + ((float)upgradeDamageIncreaseInPercent)/100f;
        upgradeCostDMG = (int)((float)upgradeCostDMG * (1f+ upgradeRiseInPercent/100f));
    }

    virtual public void UpgradeAddGun()
    {
        updateAddGun.Invoke();
        gunsList[numberOfActiveGuns].gameObject.SetActive(true);
        upgradeCostAddGun = (int)((float)upgradeCostAddGun * (1f + upgradeRiseInPercent/100f));
        numberOfActiveGuns++;
    }

    virtual public void UpgradeRange()
    {
        updateRange.Invoke();
        GetComponent<SphereCollider>().radius *= 1f + ((float)upgradeRangeIncreaseInPercent)/100f;
        upgradeCostRange = (int)((float)upgradeCostRange * (1f + upgradeRiseInPercent/100f));
    }

    virtual public void UpgradeDelay()
    {
        updateDelay.Invoke();
        shootingDelay *= 1f - ((float)upgradeDelayIncreaseInPercent / 100f);
        upgradeCostRange = (int)((float)upgradeCostRange * (1f + upgradeRiseInPercent / 100f));
    }

    virtual public ElementType GetElementType()
    {
        return type;
    }
}
