using UnityEngine;
using UnityEngine.Events;

public class EnemyHPManager : MonoBehaviour {
    
    private UnityEngine.AI.NavMeshAgent enemyAgent;
    public UnityEvent damaged;
    public UnityEvent killed;

    [Min(1f)]
    public float enemyHP = 100;
    

    private void Start() {
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Death() {
        killed.Invoke();
        Debug.Log("Enemy killed");
        Destroy(enemyAgent);
        Destroy(GetComponent<Collider>());
        Destroy(gameObject, 3);
    }

    public void ApplyDamage(float damage) {
        damaged.Invoke();
        enemyHP -= damage;

        if (enemyHP <= 0 ) {
            Death();
        }
    }
}