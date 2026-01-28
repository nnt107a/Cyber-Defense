using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

public class TechManager : MonoBehaviour
{
    public static TechManager Instance;

    [Header("Data")]
    [SerializeField] private List<TechData> unlockedTechs;
    [SerializeField] private int eCrystalCount = 1000;

    public Action OnDataChanged;
    public Action<int> OnTechUnlocked;
    public Action<int> OnECrystalChanged;
    public int ECrystalCount { get { return eCrystalCount;}}

    void Awake()
    {
        if (unlockedTechs == null)
            unlockedTechs = new List<TechData>();
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        GameManager.Instance.ApplyDataToManagers();
    }

    public bool IsUnlocked(TechData data)
    {
        Debug.Log("Is Tech Unlocked: " + data.TechName + " - " + unlockedTechs.Contains(data));
        return unlockedTechs.Contains(data);
    }

    public bool IsEnoughECrystal(TechData data)
    {
        return eCrystalCount >= data.eCrystalRequired;
    }
    public bool CanUnlock(TechData data)
    {
        if (data.prerequisites != null && unlockedTechs != null)
        {
            foreach (var prerequisite in data.prerequisites)
                if (!unlockedTechs.Contains(prerequisite))
                {
                    Debug.Log(
                        "Cannot unlock tech: "
                            + data.TechName
                            + " - missing prerequisite: "
                            + prerequisite.TechName
                    );
                    return false;
                }
        }

        return IsEnoughECrystal(data);
    }

    private void UnlockTech(TechData data)
    {
        Debug.Log("Unlock Tech: " + data.TechName);
        unlockedTechs.Add(data);

        eCrystalCount -= data.eCrystalRequired;
        OnTechUnlocked?.Invoke(eCrystalCount);
        OnDataChanged?.Invoke();

        // Update game data in GameManager
        GameManager.Instance.currentData.eCrystal = eCrystalCount;
        GameManager.Instance.currentData.unlockedTechs = unlockedTechs.Select(t => t.name).ToList();

        GameManager.Instance.SaveToDisk();
    }

    public bool TryResearch(TechData data)
    {
        if (IsUnlocked(data) || !CanUnlock(data))
            return false;

        UnlockTech(data);
        return true;
    }

    public float GetStatMultiplier(TargetType[] targetTypes, StatType statType)
    {
        float totalPercent = 0f;

        foreach (var tech in unlockedTechs)
        {
            foreach (var bonus in tech.bonuses)
            {
                if (
                    (bonus.targetType == TargetType.All || targetTypes.Contains(bonus.targetType))
                    && bonus.statType == statType
                )
                {
                    if (bonus.isPercentage)
                        totalPercent += bonus.value;
                }
            }
        }

        return 1f + totalPercent;
    }

    public EffectBonus GetEffectBonus()
    {
        float slowEffectPercent = 0f;
        float defenseReductionPercent = 0f;
        float resistanceReductionPercent = 0f;

        foreach (var tech in unlockedTechs)
        {
            foreach (var bonus in tech.bonuses)
            {
                switch (bonus.targetType)
                {
                    case TargetType.SlowEffect:
                        slowEffectPercent += bonus.value;
                        break;
                    case TargetType.Defense:
                        defenseReductionPercent += bonus.value;
                        break;
                    case TargetType.Resistance:
                        resistanceReductionPercent += bonus.value;
                        break;
                    default:
                        break;
                }
            }
        }

        return new EffectBonus
        {
            slowEffectBonus = slowEffectPercent,
            defenseReductionBonus = defenseReductionPercent,
            resistanceReductionBonus = resistanceReductionPercent
        };
    }

    public float GetFlatBonus(TargetType[] targetTypes, StatType statType)
    {
        float totalFlat = 0f;
        foreach (var tech in unlockedTechs)
        {
            foreach (var bonus in tech.bonuses)
            {
                if (
                    (bonus.targetType == TargetType.All || targetTypes.Contains(bonus.targetType))
                    && bonus.statType == statType
                )
                {
                    if (!bonus.isPercentage)
                        totalFlat += bonus.value;
                }
            }
        }
        return totalFlat;
    }

    public void SyncData(List<string> unlockedTechs, int eCrystal)
    {
        eCrystalCount = eCrystal;
        
        this.unlockedTechs.Clear();
        TechData[] allTechs = Resources.LoadAll<TechData>("ScriptableObjects/TechData");
        unlockedTechs.ForEach(id =>
        {
            TechData tech = allTechs.FirstOrDefault(t => t.name == id);
            if (tech != null)
            {
                this.unlockedTechs.Add(tech);
            }
        });

        Debug.Log("Synced Tech Data. Unlocked Techs Count: " + this.unlockedTechs.Count + ", E-Crystal: " + eCrystalCount);

        OnDataChanged?.Invoke();
    }
    public void SpendECrystal(int amount)
    {
        eCrystalCount -= amount;
        OnECrystalChanged?.Invoke(eCrystalCount);
        // Update game data in GameManager
        GameManager.Instance.currentData.eCrystal = eCrystalCount;
        GameManager.Instance.SaveToDisk();
    }
}

[System.Serializable]
public class TechUnlockPath
{
    public TechData techData;
    public Image[] connectors;
}

public class EffectBonus
{
    public float slowEffectBonus;
    public float defenseReductionBonus;
    public float resistanceReductionBonus;
}