using UnityEngine;

public class StateAttack : EnemyState
{
    protected float attackTimer = 0f;

    public override void Act(Enemy enemy)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= enemy.AttackInterval)
        {
            enemy.Attack();
            attackTimer = 0;
            return;
        }
    }
}
