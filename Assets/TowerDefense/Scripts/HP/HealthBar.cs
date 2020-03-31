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
        hp.fillAmount = health/maxEnemyHP;
    }
}
