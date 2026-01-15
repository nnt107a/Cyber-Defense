using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewTurretData", menuName = "ScriptableObjects/TurretData", order = 1)]
public class TurretData : ScriptableObject
{
    public GameObject turretPrefab;
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public GameObject projectilePrefab;
    public int eCoreCost = 50;
    public float rechargeTime = 10f;
    public TurretType[] turretTypes;
}
