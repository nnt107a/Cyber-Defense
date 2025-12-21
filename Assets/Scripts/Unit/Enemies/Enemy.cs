using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [SerializeField]
    public EnemyData enemyData;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected float currentHealth;

    protected float attackInterval;
    public float AttackInterval { get { return attackInterval; } }
    protected int laneIndex = -1;

    private EnemyState currentState;
    protected Turret target;

    public EffectController effectController;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackInterval = 1.0f / enemyData.attackSpeed;
        currentHealth = enemyData.maxHealth;
        EnemyState.stateRun = gameObject.GetComponent<StateRun>();
        EnemyState.stateAttack = gameObject.GetComponent<StateAttack>();
        currentState = GetComponent<EnemyState>();
        Debug.Log("Current state: " + currentState);
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.isLevelOnGoing)
        {
            return;
        }
        currentState.Act(this);
    }

    public void Place(int index)
    {
        laneIndex = index;
        GameManager.Instance.enemiesInLane[laneIndex].Add(this);
    }

    public void Run()
    {
        animator.SetBool("isRunning", true);
        Vector2 newPosition = rb.position + Vector2.left * enemyData.moveSpeed * Time.deltaTime * effectController.CurrentSlowMultiplier;
        rb.MovePosition(newPosition);
    }

    public virtual void Attack()
    {
        animator.SetBool("isRunning", false);
        Debug.Log("Enemy attacks.");
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
        float damageTaken = amount * (isPhysical ? (1 - Mathf.Clamp(enemyData.physicalResistance - effectController.TotalDefenseReduction, 0f, 0.7f)) : (1 - Mathf.Clamp(enemyData.magicalResistance - effectController.TotalResistanceReduction, 0f, 0.7f)));
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
        if (collider.CompareTag("House"))
        {
            GameManager.Instance.ChangeHealth(-1);
            Death();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Turret>() != null)
        {
            ChangeState(EnemyState.stateAttack);
            target = collision.gameObject.GetComponent<Turret>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Turret>() != null)
        {
            ChangeState(EnemyState.stateRun);
            target = null;
        }
    }

    /*private IEnumerator DelayALittle()
    {
        yield return new WaitForSeconds(2.5f / enemyData.moveSpeed);
        GameManager.Instance.enemiesInLane[laneIndex].Add(this);
    }*/
    public void OnSpawn()
    {
        effectController.ClearEffects();
        currentHealth = enemyData.maxHealth;
        currentState = EnemyState.stateRun;
        Debug.Log("Current state: " + currentState);
    }

    public void OnDespawn()
    {
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }
}