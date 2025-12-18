using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<SpawnEvent> spawnEvents;
    public float GetLastSpawnTime()
    {
        if (spawnEvents.Count == 0) return 0f;
        float lastTime = 0f;
        foreach (var spawnEvent in spawnEvents)
        {
            if (spawnEvent.time > lastTime)
            {
                lastTime = spawnEvent.time;
            }
        }
        return lastTime;
    }
    public float GetDuration()
    {
        if (spawnEvents.Count == 0) return 0f;
        float lastTime = 0f;
        foreach (var spawnEvent in spawnEvents)
        {
            float eventEndTime = spawnEvent.time;
            int count = spawnEvent.randomCount
                ? Random.Range(spawnEvent.countRange.x, spawnEvent.countRange.y + 1)
                : spawnEvent.fixedCount;
            if (count > 1)
            {
                eventEndTime += spawnEvent.interval * (count - 1);
            }
            if (eventEndTime > lastTime)
            {
                lastTime = eventEndTime;
            }
        }
        return lastTime;
    }
}

[System.Serializable]
public class SpawnEvent
{
    [Header("Timing")]
    public float time;                 // seconds since wave start

    [Header("Enemy Selection")]
    public List<EnemyData> enemyPool;  // pick 1 randomly
    public bool allowDuplicate = true;

    [Header("Lane Selection")]
    public bool randomLane = true;
    [Range(0, 4)] public int fixedLane;
    [Range(0, 4)] public int minLane = 0;
    [Range(0, 4)] public int maxLane = 4;

    [Header("Count")]
    public bool randomCount = false;
    public int fixedCount = 1;
    public Vector2Int countRange = new Vector2Int(1, 3);

    [Header("Spacing")]
    public float interval = 0.5f;
}