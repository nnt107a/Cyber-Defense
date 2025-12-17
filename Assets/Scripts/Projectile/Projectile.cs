using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ProjectileData projectileData;

    protected float damage;
    protected float lifetime = 5f;
    protected float lifeTimer = 0f;
    protected GameObject prefabRef;
    protected bool alive = false;
    protected Turret initator;

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
            GridCell enemyCell = Utils.GetGridCellAt(collision.transform.position);
            if (enemyCell == null)
            {
                return;
            }
            Attack(collision.gameObject, gameObject, enemyCell);
        }
    }
    public void Init(GameObject prefab, float damage, Turret turret)
    {
        prefabRef = prefab;
        this.damage = damage;
        initator = turret;
    }
    protected virtual void Attack(GameObject enemy, GameObject gameObject, GridCell gridCell)
    {
        ObjectPool.Instance.Despawn(prefabRef, gameObject);
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
