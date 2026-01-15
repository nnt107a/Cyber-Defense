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
    public TechNode[] techNodes;
    [SerializeField] TechUnlockPath[] techUnlockPaths;

    [SerializeField] private List<TechData> unlockedTechs;
    [SerializeField] private GameObject techInfoPanel;
    [SerializeField] private GameObject techTree;

    public Action<int> OnTechUnlocked;
    [SerializeField] private int eCrystalCount = 200;
    public int ECrystalCount { get { return eCrystalCount;}}
    [SerializeField] private TextMeshProUGUI eCrystalText;

    void Awake()
    {
        Instance = this;
        if (unlockedTechs == null)
            unlockedTechs = new List<TechData>();

        techNodes = GameObject.Find("TechNodeHolder").GetComponentsInChildren<TechNode>();

    }

    void Start()
    {
        SetUpNodes();
        UpdateVisuals();
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

        UpdateVisuals();
    }

    public void UnselectTech(TechData data)
    {
       techInfoPanel.SetActive(false);
    }

    private void SetUpNodes()
    {
        foreach (TechNode node in techNodes)
        {
            node.Setup(node.techData);
        }
    }

    public void UpdateVisuals()
    {
        eCrystalText.text = eCrystalCount.ToString();

        foreach (TechNode node in techNodes)
        {
            TechState stateToSet = CalculateState(node.techData);
            node.SetState(stateToSet);
        }

        foreach (TechUnlockPath path in techUnlockPaths)
        {
            foreach (Image connector in path.connectors)
            {
                if (IsUnlocked(path.techData))
                {
                    connector.color = Color.white;
                }
                else
                {
                    connector.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                }
            }
        }
    }

    private TechState CalculateState(TechData data)
    {
        if (IsUnlocked(data))
        {
            return TechState.Researched;
        }
        if (CanUnlock(data))
        {
            return TechState.Available;
        }
        else
        {
            return TechState.Locked;
        }
    }

    public void ShowTechInfo(TechData data)
    {
        techInfoPanel.SetActive(true);
        techInfoPanel.GetComponent<TechInfo>().SetUp(data);
    }

    public bool TryResearch(TechData data)
    {
        if (IsUnlocked(data) || !CanUnlock(data))
            return false;

        UnlockTech(data);
        return true;
    }

    public float GetStatMultiplier(TurretType[] turretTypes, StatType statType)
    {
        float totalPercent = 0f;

        foreach (var tech in unlockedTechs)
        {
            foreach (var bonus in tech.bonuses)
            {
                if (
                    (bonus.targetType == TurretType.All || turretTypes.Contains(bonus.targetType))
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

    public float GetFlatBonus(TurretType[] turretTypes, StatType statType)
    {
        float totalFlat = 0f;
        foreach (var tech in unlockedTechs)
        {
            foreach (var bonus in tech.bonuses)
            {
                if (
                    (bonus.targetType == TurretType.All || turretTypes.Contains(bonus.targetType))
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

}

[System.Serializable]
public class TechUnlockPath
{
    public TechData techData;
    public Image[] connectors;
}