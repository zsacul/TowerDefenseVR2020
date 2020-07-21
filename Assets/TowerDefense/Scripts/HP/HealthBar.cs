using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {

    [SerializeField]
    float maxEnemyHP;
    [SerializeField]
    Image hp;
    
    public void SetMaxHp(float h)
    {
        maxEnemyHP = h;
    }

    public void updateBar(float health) {
        //Debug.Log("New hp bar fill value: " + (health / maxEnemyHP));
        //Debug.Log("Health: " + health + "; maxEnemyHP: " + maxEnemyHP);
        hp.fillAmount = health/maxEnemyHP;
    }
}
