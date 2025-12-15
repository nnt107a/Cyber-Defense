using UnityEngine;

public class CannonBullet : Projectile
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack(GameObject gameObject)
    {
        Debug.Log("Dealing " + damage + " damage to " + gameObject.name);
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
}
