using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    public EffectData data;
    public float expirationTime;
    public void SetData(EffectData data)
    {
        this.data = data;
        this.expirationTime = Time.time + data.duration;
    }
}

public class EffectController : MonoBehaviour
{
    private List<ActiveEffect> activeEffects = new();

    public Enemy enemy;
    public float CurrentSlowMultiplier { get; private set; } = 1f;
    public float TotalDefenseReduction { get; private set; } = 0f;
    public float TotalResistanceReduction { get; private set; } = 0f;

    public void ApplyEffect(EffectData data)
    {
        ActiveEffect effect = new ActiveEffect();
        effect.SetData(data);
        activeEffects.Add(effect);
        RecalculateStats();
    }

    private void Update()
    {
        bool changed = false;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i].expirationTime <= Time.time)
            {
                activeEffects.RemoveAt(i);
                changed = true;
            }
        }

        if (changed)
            RecalculateStats();
    }

    private void RecalculateStats()
    {
        ApplySlow();
        ApplyReductions();
    }
    private void ApplySlow()
    {
        float strongestSlow = 0f;

        foreach (var debuff in activeEffects)
        {
            if (debuff.data.effectType == EffectType.Slow)
                strongestSlow = Mathf.Max(strongestSlow, debuff.data.effectValue);
        }

        if (strongestSlow > 0f)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.deepSkyBlue;
        }

        CurrentSlowMultiplier = 1f - strongestSlow;

        Debug.Log(enemy.name + " slow multiplier: " + CurrentSlowMultiplier);
    }
    private void ApplyReductions()
    {
        TotalDefenseReduction = 0f;
        TotalResistanceReduction = 0f;

        foreach (var debuff in activeEffects)
        {
            switch (debuff.data.effectType)
            {
                case EffectType.DefenseReduction:
                    TotalDefenseReduction += debuff.data.effectValue;
                    break;

                case EffectType.ResistanceReduction:
                    TotalResistanceReduction += debuff.data.effectValue;
                    break;
            }
        }
    }

}

