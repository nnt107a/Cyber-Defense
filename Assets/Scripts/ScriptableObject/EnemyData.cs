using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public int eCoreDrop = 50;
}
