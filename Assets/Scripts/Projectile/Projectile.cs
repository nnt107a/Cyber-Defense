using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 10f;

    protected float lifetime = 5f;
    protected float lifeTimer = 0f;

    protected virtual void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
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
    protected virtual void Attack()
    {
        Destroy(gameObject);
    }
}
