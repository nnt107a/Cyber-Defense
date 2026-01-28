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
    protected Collider2D enemyCollider;

    protected float attackInterval;
    public float AttackInterval { get { return attackInterval; } }
    protected int laneIndex = -1;

    private EnemyState currentState;
    protected Turret target;
    protected bool isDying = false;
    protected bool hit = false;

    public EffectController effectController;

    protected virtual void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackInterval = 1.0f / enemyData.attackSpeed;
        currentHealth = enemyData.maxHealth;
        EnemyState.stateRun = gameObject.GetComponent<StateRun>();
        EnemyState.stateAttack = gameObject.GetComponent<StateAttack>();
        currentState = GetComponent<EnemyState>();
        Debug.Log("Current state: " + currentState);
    }

    public void EnterPreviewMode()
    {
        enabled = false;
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.isLevelOnGoing)
        {
            return;
        }
        if (isDying || hit)
        {
            return;
        }
        currentState.Act(this);
    }

    public void Place(int index)
    {
        hit = false;
        isDying = false;
        laneIndex = index;
        GameManager.Instance.enemiesInLane[laneIndex].Add(this);
        enemyCollider.enabled = true;
    }

    public void Run()
    {
        animator.SetBool("isRunning", true);
        transform.position = transform.position + Vector3.left * enemyData.moveSpeed * Time.deltaTime * effectController.CurrentSlowMultiplier;
    }

    public virtual void Attack()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("attack");
    }
    public void DealDamage()
    {
        Debug.Log("Enemy attacks.");
        target?.TakeDamage(enemyData.attackDamage);
        if (currentState is StateAttack stateAttack)
        {
            stateAttack.ResetAttackTimer();
        }
    }

    protected virtual void Death(bool defeated = true)
    {
        if (this is Gloomslime gloomslime)
        {
            if (gloomslime.isParent)
            {
                Gloomslime child1 = ObjectPool
                    .Instance.Spawn(gloomslime.childPrefab, transform.position + Vector3.left * 0.2f, Quaternion.identity)
                    .GetComponent<Gloomslime>();
                child1.Place(laneIndex);
                child1.isParent = false;

                Gloomslime child2 = ObjectPool
                    .Instance.Spawn(gloomslime.childPrefab, transform.position + Vector3.right * 0.2f, Quaternion.identity)
                    .GetComponent<Gloomslime>();
                child2.Place(laneIndex);
                child2.isParent = false;
            }
        }
        if (defeated)
        {
            LevelManager.Instance.ChangeECoreCount(enemyData.eCoreDrop);
        }
        Debug.Log("Enemy died.");
        GameManager.Instance.enemiesInLane[laneIndex].Remove(this);
        ObjectPool.Instance.Despawn(enemyData.enemyPrefab, gameObject);
    }
    public void OnDeathAnimEnd()
    {
        Death();
    }
    protected void PlayDeathAnim()
    {
        if (isDying)
        {
            return;
        }
        enemyCollider.enabled = false;
        isDying = true;
        animator.ResetTrigger("takeHit");
        animator.SetTrigger("death");
    }
    public void TakeDamage(int amount, bool isPhysical = false)
    {
        if (!hit)
        {
            animator.SetTrigger("takeHit");
            hit = true;
        }
        float damageTaken = amount * (isPhysical ? (1 - Mathf.Clamp(enemyData.physicalResistance - effectController.TotalDefenseReduction, 0f, 0.7f)) : (1 - Mathf.Clamp(enemyData.magicalResistance - effectController.TotalResistanceReduction, 0f, 0.7f)));
        Debug.Log("Enemy took " + (isPhysical ? "physic" : "magic") + " damage: " + damageTaken);
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            PlayDeathAnim();
        }
    }
    public void ResetHitState()
    {
        hit = false;
        animator.SetBool("isRunning", false);
    }
    public void ResetHitStateToMove()
    {
        /*hit = false;*/
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("House"))
        {
            GameManager.Instance.ChangeHealth(-1);
            Death(false);
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
        if (newState == EnemyState.stateAttack)
        {
            animator.SetBool("isRunning", false);
        }
        currentState = newState;
    }
    public void SetLaneIndex(int index)
    {
        this.laneIndex = index;
    }
}