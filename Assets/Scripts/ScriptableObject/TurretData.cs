using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewTurretData", menuName = "ScriptableObjects/TurretData", order = 1)]
public class TurretData : ScriptableObject
{
    public GameObject turretPrefab;
    public int turretID;
    public string turretName;
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public GameObject projectilePrefab;
    public int eCoreCost = 50;
    public float rechargeTime = 10f;
    public float activationDelay = 10f;
    public float radius = 1f;
    public Sprite turretIcon;
    public EffectData specialEffect;
    public TargetType[] targetType;
    public string description;
    public string vnDescription;
}
