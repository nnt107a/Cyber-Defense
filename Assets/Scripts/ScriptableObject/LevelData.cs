using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Data")]
public class LevelData : ScriptableObject
{
    public List<WaveEntry> waves;
}

[System.Serializable]
public class WaveEntry
{
    public WaveData wave;
    public float intervalAfterWave = 5f;
}