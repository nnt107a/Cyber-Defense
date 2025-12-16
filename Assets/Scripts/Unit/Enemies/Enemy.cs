using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public EnemyData enemyData;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected float currentHealth;
    protected float attackTimer = 0f;
    protected float attackInterval;
    protected int laneIndex = -1;

    protected bool enemiesInLane = true;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackInterval = 1.0f / enemyData.attackSpeed;
        currentHealth = enemyData.maxHealth;
    }

    protected virtual void Update()
    {
        // if (laneIndex == -1)
        // {
        //     return;
        // }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && enemiesInLane)
        {
            Attack();
            attackTimer = -10f;
            return;
        }
        Run();
    }

    public void Place(int index)
    {
        laneIndex = index;
    }

    protected void Run()
    {
        animator.SetBool("isRunning", true);
        Vector2 newPosition = rb.position + Vector2.left * enemyData.moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    protected virtual void Attack()
    {
        animator.SetBool("isRunning", false);
        Debug.Log("Enemy attacks.");
        attackTimer = 0f;
        animator.SetTrigger("attack");
    }

    protected void Death()
    {
        LevelManager.Instance.ChangeECoreCount(enemyData.eCoreDrop);
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Enemy took damage: " + amount);
        currentHealth -= amount;
        animator.SetTrigger("takeHit");
        if (currentHealth <= 0)
        {
            animator.ResetTrigger("takeHit");
            animator.SetTrigger("death");
        }
    }
}
