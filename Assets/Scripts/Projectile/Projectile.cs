using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ProjectileData projectileData; 
    [SerializeField] private LayerMask gridCellLayer;

    protected float damage;
    protected float lifetime = 5f;
    protected float lifeTimer = 0f;
    protected GameObject prefabRef;
    protected bool alive = false;

    protected virtual void Update()
    {
        if (!alive)
        {
            return;
        }
        transform.Translate(Vector3.right * projectileData.speed * Time.deltaTime);
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit: " + collision.gameObject.name);
            GridCell enemyCell = GetGridCellAt(collision.transform.position);
            Debug.Log("Enemy is at cell: " + enemyCell.x + ", " + enemyCell.y);
            Attack(gameObject, enemyCell);
        }
    }
    public void Init(GameObject prefab, float damage)
    {
        prefabRef = prefab;
        this.damage = damage;
    }
    protected virtual void Attack(GameObject gameObject, GridCell gridCell)
    {
        ObjectPool.Instance.Despawn(prefabRef, gameObject);
    }
    protected GridCell GetGridCellAt(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos, gridCellLayer);
        return hit ? hit.GetComponent<GridCell>() : null;
    }
    protected bool IsWithinGridRadius(GridCell center, GridCell other, int radius)
    {
        int dx = Mathf.Abs(center.x - other.x);
        int dy = Mathf.Abs(center.y - other.y);

        return Mathf.Max(dx, dy) <= radius;
    }

    public virtual void OnSpawn()
    {
        lifeTimer = 0f;
        alive = true;
    }

    public virtual void OnDespawn()
    {
        alive = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.radius);
    }
}
