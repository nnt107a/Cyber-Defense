using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float attackDamage = 10f;
    /*[SerializeField] private float attackRange = 5f;*/
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected GameObject projectilePrefab;
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
        attackInterval = 1.0f / attackSpeed;
        currentHealth = maxHealth;
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
        GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log($"Attacking enemy for {attackDamage} damage.");
    }
}
