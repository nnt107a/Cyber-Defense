using UnityEngine;

public class CoreReactor : Turret
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        if (!GameManager.Instance.isLevelOnGoing)
        {
            return;
        }
        if (laneIndex == -1)
        {
            return;
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            InitAttack();
        }
    }
    protected override void InitAttack()
    {
        Debug.Log("Core Reactor spawn ECore.");
        UIFloatingText.Instance.ShowFloatingText("+" + turretData.attackDamage.ToString(), firePoint.position, Color.gold);
        LevelManager.Instance.ChangeECoreCount((int)turretData.attackDamage);
        attackTimer = 0f;
    }
    protected override void Attack()
    {
        base.Attack();
    }
}
