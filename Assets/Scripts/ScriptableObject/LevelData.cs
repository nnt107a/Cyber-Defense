using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Data")]
public class LevelData : ScriptableObject
{
    public List<WaveEntry> waves; 
    
    [Header("Loadout Preview")]
    public int previewEnemyCount = 10;
    public float GetTotalDuration()
    {
        float total = 0f;

        for (int i = 0; i < waves.Count; i++)
        {
            total += waves[i].wave.GetDuration();

            if (i < waves.Count - 1)
                total += waves[i].intervalAfterWave;
        }

        return total;
    }
    public CutsceneData introCutscene;
    public CutsceneData outroCutscene;
}

[System.Serializable]
public class WaveEntry
{
    public WaveData wave;
    public float intervalAfterWave = 5f;
}