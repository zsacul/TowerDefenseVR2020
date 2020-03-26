using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHPManager : MonoBehaviour {
    
    private UnityEngine.AI.NavMeshAgent enemyAgent;
    public UnityEvent damaged;
    public UnityEvent killed;
    
    [SerializeField]
    GameObject targetPoint;
    
    [Min(1f)]
    public float enemyHP = 100f;

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

    public void ApplyDamage(Bullet b) {
        damaged.Invoke();
        if (b.GetBulletType() == TowerType.fire)
            enemyHP -= b.GetDamage() * 2;
        else
            enemyHP -= b.GetDamage();

        GetComponent<HealthBar>().updateBar(enemyHP);

        b.SetReadyToDestroy();


        if (enemyHP <= 0 ) {
            Death();
        }


    }

    public void ApplyDamage(int damage)
    {
        damaged.Invoke();
        enemyHP -= damage;

        if (enemyHP <= 0)
        {
            Death();
        }
    }

    public GameObject GetTargetPoint(){
        return targetPoint;
    }
}