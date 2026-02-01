using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Data")]
public class LevelData : ScriptableObject
{
    public List<WaveEntry> waves;
    public GameObject environmentalEffectPrefab;
    [Header("Visuals")]
    public MapThemeData theme;
    [HideInInspector]
    public List<SavedTile> mapLayout = new List<SavedTile>();
    
    [Header("Loadout Preview")]
    public int previewEnemyCount = 10;
    public float GetTotalDuration()
    {
        float total = 0f;

        for (int i = 0; i < waves.Count - 1; i++)
        {
            total += waves[i].wave.GetDuration();

            if (i < waves.Count - 1)
                total += waves[i].intervalAfterWave;
        }
        total += waves[waves.Count - 1].wave.spawnEvents[0].time;

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

[System.Serializable]
public class SavedTile
{
    public Vector3Int position;
    public TileBase tileAsset;
}
