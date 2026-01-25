using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "Turret/TurretData")]
public class TurretData : ScriptableObject
{

    public GameObject turretPrefab;
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
}
