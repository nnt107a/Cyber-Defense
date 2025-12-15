using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ProjectileData projectileData;

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
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name);
        Attack();
    }
    public void Init(GameObject prefab)
    {
        prefabRef = prefab;
    }
    protected virtual void Attack()
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
}
