using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {

    [SerializeField]
    float maxEnemyHP;
    [SerializeField]
    Image hp;

    private void Start()
    {
    }

    public void updateBar(float health) {
        hp.fillAmount = health/maxEnemyHP;
    }
}
