using UnityEngine;
using UnityEngine.Events;

public class EnemyHPManager : MonoBehaviour {
    
    private UnityEngine.AI.NavMeshAgent enemyAgent;
<<<<<<< HEAD:Assets/TowerDefense/Scripts/EnemyHPManager.cs
    public UnityEvent damaged;
    public UnityEvent killed;
    
    [SerializeField]
    GameObject targetPoint;
    
    [Min(1f)]
    public float enemyHP = 100;
=======

    public HealthBar healthBar;
        
    [Min(1f)]
    public float enemyMaxHP = 100;
    
    private float enemyHP;  
>>>>>>> origin/Dawid/HealthBars:Assets/TowerDefense/Scripts/HP/EnemyHPManager.cs

    private void Start() {
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyHP = enemyMaxHP;
    }

    private void Death() {
        killed.Invoke();
        Debug.Log("Enemy killed");
        Destroy(enemyAgent);
        Destroy(GetComponent<Collider>());
        Destroy(gameObject, 3);
    }

<<<<<<< HEAD:Assets/TowerDefense/Scripts/EnemyHPManager.cs
    public void ApplyDamage(Bullet b) {
        damaged.Invoke();
        if (b.GetBulletType() == TowerType.fire)
            enemyHP -= b.GetDamage() * 2;
        else
            enemyHP -= b.GetDamage();

        b.SetReadyToDestroy();
=======
    public void ApplyDamage(float damage) {
        
        enemyHP -= damage;
        healthBar.updateBar(enemyHP / enemyMaxHP);
>>>>>>> origin/Dawid/HealthBars:Assets/TowerDefense/Scripts/HP/EnemyHPManager.cs

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