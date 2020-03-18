using UnityEngine;

public class EnemyHPManager : MonoBehaviour {
    
    private UnityEngine.AI.NavMeshAgent enemyAgent;

    public HealthBar healthBar;
        
    [Min(1f)]
    public float enemyMaxHP = 100;
    
    private float enemyHP;  

    private void Start() {
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyHP = enemyMaxHP;
    }

    private void Death() {
        Debug.Log("Enemy killed");
        Destroy(enemyAgent.gameObject);
    }

    public void ApplyDamage(float damage) {
        
        enemyHP -= damage;
        healthBar.updateBar(enemyHP / enemyMaxHP);

        if (enemyHP <= 0 ) {
            Death();
        }
    }
}