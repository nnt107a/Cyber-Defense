using System.Collections.Generic;
using UnityEngine;


public class TurretEffectController : MonoBehaviour
{
    private List<ActiveEffect> activeEffects = new();

    public Turret turret;
    public float CurrentSlowMultiplier { get; private set; } = 1f;
    public Color turretErrorColor = new Color(1f, 0.5f, 0.5f, 1f);

    public void ApplyEffect(EffectData data, int id)
    {
        ActiveEffect effect = new ActiveEffect();
        effect.SetData(data, id);
        if (
            activeEffects.Exists(e =>
                e.data.effectType == data.effectType && e.instanceId == effect.instanceId
            )
        )
        {
            var existingEffect = activeEffects.Find(e =>
                e.data.effectType == data.effectType && e.instanceId == effect.instanceId
            );
            existingEffect.expirationTime = data.duration > 0 ? Time.time + data.duration : Mathf.Infinity;
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
        turret.GetComponent<SpriteRenderer>().color = Color.white;
        ApplySlow();
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
            turret.GetComponent<SpriteRenderer>().color = turretErrorColor;
        }

        CurrentSlowMultiplier = 1f - strongestSlow;
    }

    public void ClearEffects()
    {
        activeEffects.Clear();
        RecalculateStats();
    }
}
