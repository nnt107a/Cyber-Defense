using UnityEngine;

public class CannonBullet : Projectile
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack(GameObject enemy, GameObject gameObject, GridCell gridCell)
    {
        Debug.Log("Dealing " + damage + " damage to " + gameObject.name);
        enemy.GetComponent<Enemy>()?.TakeDamage((int)damage);
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
