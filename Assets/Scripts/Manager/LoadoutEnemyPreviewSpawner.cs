using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadoutEnemyPreviewSpawner : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    [SerializeField] Transform[] previewSpawnPoints;

    List<GameObject> spawnedPreviews = new();

    public void SpawnPreviews()
    {
        Clear();

        levelData = WaveManager.Instance.levelData;

        var pool = GeneratePreviewDistribution(CollectPreviewEnemies(levelData), levelData.previewEnemyCount);

        int i = 0;
        foreach (var kv in pool)
        {
            for (int c = 0; c < kv.Value; c++)
            {
                Transform point = previewSpawnPoints[i++];

                GameObject go = Instantiate(
                    kv.Key.enemyPrefab,
                    point.position,
                    Quaternion.identity
                );

                Enemy enemy = go.GetComponent<Enemy>();
                enemy.EnterPreviewMode();
                spawnedPreviews.Add(go);
            }
        }
    }

    public void Clear()
    {
        foreach (var go in spawnedPreviews)
            Destroy(go);

        spawnedPreviews.Clear();
    }
    public static List<EnemyData> CollectPreviewEnemies(LevelData level)
    {
        List<EnemyData> pool = new();

        foreach (var wave in level.waves)
        {
            foreach (var e in wave.wave.spawnEvents)
            {
                pool.AddRange(e.enemyPool);
            }
        }

        return pool;
    }
    public static Dictionary<EnemyData, int> GeneratePreviewDistribution(List<EnemyData> enemyPool, int previewCount)
    {
        Dictionary<EnemyData, int> weightMap = new();

        foreach (var e in enemyPool)
        {
            if (!weightMap.ContainsKey(e))
                weightMap[e] = 0;
            weightMap[e]++;
        }

        int uniqueCount = weightMap.Count;

        if (previewCount <= uniqueCount)
        {
            Dictionary<EnemyData, int> minimal = new();
            foreach (var kv in weightMap)
            {
                minimal[kv.Key] = 1;
                if (--previewCount <= 0)
                    break;
            }
            return minimal;
        }

        Dictionary<EnemyData, int> result = new();

        foreach (var kv in weightMap)
            result[kv.Key] = 1;

        int remaining = previewCount - uniqueCount;

        int totalWeight = enemyPool.Count;

        Dictionary<EnemyData, float> idealExtra = new();
        foreach (var kv in weightMap)
        {
            idealExtra[kv.Key] =
                (float)kv.Value / totalWeight * remaining;
        }

        int used = 0;

        foreach (var kv in idealExtra)
        {
            int floor = Mathf.FloorToInt(kv.Value);
            result[kv.Key] += floor;
            used += floor;
        }

        int leftovers = remaining - used;

        var sorted = idealExtra
            .OrderByDescending(kv => kv.Value - Mathf.Floor(kv.Value))
            .ToList();

        int i = 0;
        while (leftovers-- > 0)
        {
            result[sorted[i % sorted.Count].Key]++;
            i++;
        }

        return result;
    }
}
