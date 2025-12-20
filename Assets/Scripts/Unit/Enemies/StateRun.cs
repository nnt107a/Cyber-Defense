using UnityEngine;

public class StateRun : EnemyState
{
    public override void Act(Enemy enemy)
    {
        enemy.Run();
    }
}
