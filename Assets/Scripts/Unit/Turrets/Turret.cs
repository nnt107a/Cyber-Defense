using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] public TurretData turretData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform floatingTextPoint;

    protected Animator animator;
    protected float currentHealth;
    protected float attackTimer = 0f;
    protected float attackInterval;
    protected int laneIndex = -1;
    protected GridCell gridCell;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        attackInterval = 1.0f / turretData.attackSpeed;
        currentHealth = turretData.maxHealth;
    }
    protected virtual void Update()
    {
        if (!GameManager.Instance.isLevelOnGoing)
        {
            return;
        }
        if (laneIndex == -1)
        {
            return;
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && HasEnemyInFront())
        {
            InitAttack();
            attackTimer = -10f;
        }
    }
    bool HasEnemyInFront()
    {
        var enemies = GameManager.Instance.enemiesInLane[laneIndex];

        foreach (var enemy in enemies)
        {
            if (enemy.transform.position.x > transform.position.x)
                return true;
        }
        return false;
    }
    public void Place(int index, GridCell gridCell)
    {
        laneIndex = index;
        this.gridCell = gridCell;
    }
    protected virtual void InitAttack()
    {
        Debug.Log("Turret initiating attack.");
        animator.SetTrigger("attack");
        SoundManager.Instance.PlayTurretShootSound();
    }
    protected virtual void Attack()
    {
        attackTimer = 0f;
        GameObject go = ObjectPool.Instance.Spawn(turretData.projectilePrefab, firePoint.position, Quaternion.identity);
        go.GetComponent<Projectile>().Init(turretData.projectilePrefab, turretData.attackDamage, this);
    }
    public void TakeDamage(float damage)
    {
        //Add effect
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            gridCell.RemoveTurret();
        }
    }
    public void ShowFloatingText(string text, Color color)
    {
        UIFloatingText.Instance.ShowFloatingText(text, floatingTextPoint.position, color);
    }
    private void OnMouseDown()
    {
        gridCell.OnMouseDown();
    }
}
