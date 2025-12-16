using System;
using UnityEngine;

public enum EffectType
{
    Venom,
    Slow,
    DefenseReduction,
    ResistanceReduction
}

[Serializable]
[CreateAssetMenu(fileName = "NewEffectData", menuName = "ScriptableObjects/EffectData", order = 1)]
public class EffectData : ScriptableObject
{
    public EffectType effectType;
    public float duration = 3f;
    public float effectValue = 10f;
}
