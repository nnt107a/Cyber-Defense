using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackDamage = 10f;
    /*[SerializeField] private float attackRange = 5f;*/
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private GameObject projectilePrefab;

    private float currentHealth;
    private float attackTimer = 0f;
    private float attackInterval;
    private int laneIndex = -1;

    private bool enemiesInLane = true;

    protected virtual void Awake()
    {
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
            Attack();
            attackTimer = 0f;
        }
    }
    public void Place(int index)
    {
        laneIndex = index;
    }
    protected virtual void Attack()
    {
        GameObject go = Instantiate(projectilePrefab, transform.position + Vector3.right * .25f + Vector3.up * .25f, Quaternion.identity);
        Debug.Log($"Attacking enemy for {attackDamage} damage.");
    }
}
