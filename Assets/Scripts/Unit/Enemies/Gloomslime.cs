using UnityEngine;

public class Gloomslime : Enemy
{
    [SerializeField]
    GameObject childPrefab;
    [SerializeField]
    private bool isParent = true;

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

    protected override void Death()
    {
        if (isParent)
        {
            Gloomslime child1 = ObjectPool
                .Instance.Spawn(childPrefab, transform.position + Vector3.left * 0.2f, Quaternion.identity)
                .GetComponent<Gloomslime>();
            child1.Place(laneIndex);
            child1.isParent = false;

            Gloomslime child2 = ObjectPool
                .Instance.Spawn(childPrefab, transform.position + Vector3.right * 0.2f, Quaternion.identity)
                .GetComponent<Gloomslime>();
            child2.Place(laneIndex);
            child2.isParent = false;
        }
        base.Death();
    }

}
