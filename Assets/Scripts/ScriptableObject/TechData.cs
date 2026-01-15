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
    Cost,
    All,
}

public enum TurretType
{
    All,
    Cannon,
    Trap,
    Support,
    Physical,
    Elemental,
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

    private void OnValidate()
    {
        if (bonuses == null)
            return;

        foreach (var bonus in bonuses)
        {
            if (!IsStatValidForType(techType, bonus.statType))
            {
                Debug.LogError(
                    $"[DATA ERROR] Tech '{name}' is belongs to '{techType}' but is assigned stat '{bonus.statType}' is not valid!"
                );
            }
        }
    }

    private bool IsStatValidForType(TechType type, StatType stat)
    {
        switch (type)
        {
            case TechType.General:
                // Chỉ cho phép các stat này
                return stat == StatType.MaxHealth
                    || stat == StatType.Cost
                    || stat == StatType.AttackSpeed
                    || stat == StatType.All;

            case TechType.Special:
                // Chỉ cho phép các stat này
                return stat == StatType.AttackDamage;

            default:
                return false;
        }
    }

}
