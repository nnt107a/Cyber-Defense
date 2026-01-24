using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public TurretData turretData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform floatingTextPoint;

    protected Animator animator;
    protected float currentHealth;
    protected float currentDamage;
    protected float attackTimer = 0f;
    protected float attackInterval;
    protected int laneIndex = -1;
    protected GridCell gridCell;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        UpdateStats();
    }
    public void UpdateStats()
    {
        // Calculation with Tech bonuses
        float damageMult = TechManager.Instance.GetStatMultiplier(turretData.targetType, StatType.AttackDamage);
        float damageFlat = TechManager.Instance.GetFlatBonus(turretData.targetType, StatType.AttackDamage);

        currentDamage = (turretData.attackDamage + damageFlat) * damageMult;

        float healthMult = TechManager.Instance.GetStatMultiplier( turretData.targetType, StatType.MaxHealth);
        float healthFlat = TechManager.Instance.GetFlatBonus(turretData.targetType, StatType.MaxHealth);

        currentHealth = (turretData.maxHealth + healthFlat) * healthMult;

        float speedMult = TechManager.Instance.GetStatMultiplier(turretData.targetType, StatType.AttackSpeed);
        attackInterval = 1.0f / speedMult / turretData.attackSpeed;

        Debug.Log($"{gameObject.name} Stats Updated: Dmg={currentDamage}, AttackInterval={attackInterval}, Health={currentHealth}");
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
    }
    protected virtual void Attack()
    {
        attackTimer = 0f;
        GameObject go = ObjectPool.Instance.Spawn(turretData.projectilePrefab, firePoint.position, Quaternion.identity);
        go.GetComponent<Projectile>().Init(turretData.projectilePrefab, currentDamage, this);
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
