using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public static StateRun stateRun;
    public static StateAttack stateAttack;

    public virtual void Act(Enemy enemy) { }
}
