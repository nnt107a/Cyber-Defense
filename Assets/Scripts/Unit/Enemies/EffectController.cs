using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect
{
    public EffectData data;
    public float expirationTime;
    public int instanceId;
    public void SetData(EffectData data, int instanceId)
    {
        this.data = data;
        this.expirationTime = Time.time + data.duration;
        this.instanceId = instanceId;
    }
}

public class EffectController : MonoBehaviour
{
    private List<ActiveEffect> activeEffects = new();

    public Enemy enemy;
    public float CurrentSlowMultiplier { get; private set; } = 1f;
    public float TotalDefenseReduction { get; private set; } = 0f;
    public float TotalResistanceReduction { get; private set; } = 0f;
    
    private StatusEffectHandler statusEffectHandler;
    private void Awake()
    {
        statusEffectHandler = GetComponent<StatusEffectHandler>();
    }
    public void ApplyEffect(EffectData data, Turret turret)
    {
        ActiveEffect effect = new ActiveEffect();
        effect.SetData(data, turret.GetInstanceID());
        if (activeEffects.Exists(e => e.data.effectType == data.effectType && e.instanceId == effect.instanceId))
        {
            var existingEffect = activeEffects.Find(e => e.data.effectType == data.effectType && e.instanceId == effect.instanceId);
            existingEffect.expirationTime = Time.time + data.duration;
            RecalculateStats();
            return;
        }
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
        enemy.GetComponent<SpriteRenderer>().color = Color.white;
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

        statusEffectHandler?.PlaySlowEffect(strongestSlow > 0f);
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
        if (TotalDefenseReduction > 0f || TotalResistanceReduction > 0f)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.purple;
        }

        statusEffectHandler?.PlayReduceResEffect(TotalDefenseReduction > 0f || TotalResistanceReduction > 0f);
    }
    public void ClearEffects()
    {
        activeEffects.Clear();
        RecalculateStats();
    }
}

