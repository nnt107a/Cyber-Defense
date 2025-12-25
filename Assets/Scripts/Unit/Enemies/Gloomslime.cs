using UnityEngine;

public class Gloomslime : Enemy
{
    [SerializeField]
    public GameObject childPrefab;
    [SerializeField]
    public bool isParent = true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();
    }
}
