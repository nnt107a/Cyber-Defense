using UnityEngine;

public class VenomSpireBullet : Projectile
{
    [SerializeField] private GameObject venomSpireZonePrefab;
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack(GameObject enemy, GameObject gameObject, GridCell gridCell)
    {
        Debug.Log("Venom Spire Attack Hit " + gameObject.name + " for " + damage + " damage");
        enemy.GetComponent<Enemy>()?.TakeDamage((int)damage);
        GameObject venomZoneObj = ObjectPool.Instance.Spawn(venomSpireZonePrefab, gridCell.transform.position, Quaternion.identity);
        venomZoneObj.transform.localScale = new Vector3(projectileData.radius * 2 + 1, projectileData.radius * 2 + (Utils.IsTopLane(gridCell) || Utils.IsBottomLane(gridCell) ? 0 : 1), 1);
        venomZoneObj.transform.position += (Utils.IsTopLane(gridCell) ? new Vector3(0, -0.5f, 0) : (Utils.IsBottomLane(gridCell) ? new Vector3(0, 0.5f, 0) : Vector3.zero));
        venomZoneObj.GetComponent<VenomSpireZone>().Init(venomSpireZonePrefab);
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
