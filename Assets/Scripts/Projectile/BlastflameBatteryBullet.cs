using UnityEngine;

public class BlastflameBatteryBullet : Projectile
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack(GameObject enemy, GameObject gameObject, GridCell gridCell)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            projectileData.radius * 1.5f
        );

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            GridCell enemyCell = Utils.GetGridCellAt(hit.transform.position);

            if (enemyCell == null)
                continue;

            if (Utils.IsWithinGridRadius(gridCell, enemyCell, (int)projectileData.radius))
            {
                hit.GetComponent<Enemy>()?.TakeDamage((int)damage);
                Debug.Log(
                    $"Explosion hit enemy {hit.name} at cell ({enemyCell.x},{enemyCell.y}), deal {damage}"
                );
            }
        }
        base.Attack(enemy, gameObject, gridCell);
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }
}
