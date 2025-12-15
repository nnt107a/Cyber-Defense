using UnityEngine;

public class BlastflameBatteryBullet : Projectile
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack(GameObject gameObject)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            projectileData.radius
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log($"Explosion hit enemy: {hit.name}, deal {damage}");
            }
        }
        base.Attack(gameObject);
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.radius);
    }
}
