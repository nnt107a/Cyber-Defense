using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public Sprite enemyIcon;
    public string enemyName;
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public float physicalResistance = 0f;
    public float magicalResistance = 0f;
    public int eCoreDrop = 50;
    public string description;
    public string vnDescription;
}
