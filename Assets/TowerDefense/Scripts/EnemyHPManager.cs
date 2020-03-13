using UnityEngine;

public class EnemyHPManager : MonoBehaviour {
    
    private UnityEngine.AI.NavMeshAgent enemyAgent;

    [Min(1f)]
    public float enemyHP = 100;
    

    private void Start() {
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Death() {
        Debug.Log("Enemy killed");
        Destroy(enemyAgent.gameObject);
    }

    public void ApplyDamage(float damage) {
        
        enemyHP -= damage;

        if (enemyHP <= 0 ) {
            Death();
        }
    }
}