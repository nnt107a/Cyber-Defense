using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [SerializeField]
    public EnemyData enemyData;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected float currentHealth;
    protected float attackTimer = 0f;
    protected float attackInterval;
    protected int laneIndex = -1;

    public EffectController effectController;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackInterval = 1.0f / enemyData.attackSpeed;
        currentHealth = enemyData.maxHealth;
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
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
        Vector2 newPosition = rb.position + Vector2.left * enemyData.moveSpeed * Time.deltaTime * effectController.CurrentSlowMultiplier;
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
        GameManager.Instance.enemiesInLane[laneIndex].Remove(this);
        ObjectPool.Instance.Despawn(enemyData.enemyPrefab, gameObject);
    }

    public void TakeDamage(int amount, bool isPhysical = false)
    {
        float damageTaken = amount * (isPhysical ? (1 - Mathf.Clamp01(/*- enemyData.physicalResistance*/ - effectController.TotalDefenseReduction)) : (1 - Mathf.Clamp01(/*- enemyData.magicalResistance*/ - effectController.TotalResistanceReduction)));
        Debug.Log("Enemy took " + (isPhysical ? "physic" : "magic") + " damage: " + damageTaken);
        currentHealth -= damageTaken;
        animator.SetTrigger("takeHit");
        if (currentHealth <= 0)
        {
            animator.ResetTrigger("takeHit");
            animator.SetTrigger("death");
            Death();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Grid"))
        {
            if (collider.GetComponent<GridCell>().x == GridManager.width - 1)
            {
                GameManager.Instance.enemiesInLane[laneIndex].Add(this);
            }
        }
    }
    public void OnSpawn()
    {
        effectController.ClearEffects();
        currentHealth = enemyData.maxHealth;
        attackTimer = 0f;
    }

    public void OnDespawn()
    {
    }
}