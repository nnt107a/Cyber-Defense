using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewEffectData", menuName = "ScriptableObjects/EffectData", order = 1)]
public class EffectData : ScriptableObject
{
    public float duration = 3f;
    public float effectValue = 10f;
}
