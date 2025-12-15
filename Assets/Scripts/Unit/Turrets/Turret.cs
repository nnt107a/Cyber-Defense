using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected TurretData turretData;
    [SerializeField] protected Transform firePoint;

    protected Animator animator;
    protected float currentHealth;
    protected float attackTimer = 0f;
    protected float attackInterval;
    protected int laneIndex = -1;

    protected bool enemiesInLane = true;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        attackInterval = 1.0f / turretData.attackSpeed;
        currentHealth = turretData.maxHealth;
    }
    protected virtual void Update()
    {
        if (laneIndex == -1)
        {
            return;
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && enemiesInLane)
        {
            InitAttack();
            attackTimer = -10f;
        }
    }
    public void Place(int index)
    {
        laneIndex = index;
    }
    protected virtual void InitAttack()
    {
        Debug.Log("Turret initiating attack.");
        animator.SetTrigger("attack");
    }
    protected virtual void Attack()
    {
        attackTimer = 0f;
        GameObject go = ObjectPool.Instance.Spawn(turretData.projectilePrefab, firePoint.position, Quaternion.identity);
        go.GetComponent<Projectile>().Init(turretData.projectilePrefab, turretData.attackDamage);
    }
}
