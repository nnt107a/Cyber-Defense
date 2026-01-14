using System;
using System.Collections.Generic;
using UnityEngine;

public enum TechType
{
    General,
    Special,
}

public enum StatType
{
    None,
    AttackDamage,
    AttackSpeed,
    MaxHealth,
    All,
}

public enum TurretType
{
    All,
    Cannon,
    Trap,
    Support,
}

[Serializable]
public class TechBonus
{
    public TurretType targetType;
    public StatType statType;
    public float value;
    public bool isPercentage;
}

[Serializable]
[CreateAssetMenu(fileName = "NewTechData", menuName = "ScriptableObjects/TechData", order = 1)]
public class TechData : ScriptableObject
{
    public string TechName = "New Tech";
    public string TechDescription = "";
    public TechType techType = TechType.General;
    public List<TechBonus> bonuses;
    public Sprite techIcon;
    public int eCrystalRequired = 50;
    public TechData[] prerequisites = Array.Empty<TechData>();

}
