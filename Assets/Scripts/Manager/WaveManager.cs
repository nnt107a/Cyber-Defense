using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("Level")]
    public LevelData levelData;

    [Header("Spawn")]
    [SerializeField] private Transform[] spawnPos;

    private int currentWaveIndex;

    public Action<bool> OnWaveWarning; 
    public event Action<int> OnWaveFirstSpawnEvent;
    private bool newWaveStart = true;

    private HashSet<int> activeSpawnEvents = new HashSet<int>();
    private int spawnEventIdCounter = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void StartWaveSpawn()
    {
        UILevelStateBar.Instance.Build(levelData);
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        for (currentWaveIndex = 0;
             currentWaveIndex < levelData.waves.Count;
             currentWaveIndex++)
        {
            WaveEntry entry = levelData.waves[currentWaveIndex];
            bool isFinalWave =
                currentWaveIndex == levelData.waves.Count - 1;

            yield return StartCoroutine(RunWave(entry.wave, isFinalWave, currentWaveIndex));

            yield return new WaitForSeconds(entry.intervalAfterWave);
        }
        GameManager.Instance.enemiesSpawnedCompletely = true;
    }

    IEnumerator RunWave(WaveData wave, bool isFinalWave, int currentWaveIndex)
    {
        int eventIndex = 0;

        List<SpawnEvent> events =
            new List<SpawnEvent>(wave.spawnEvents);

        events.Sort((a, b) => a.time.CompareTo(b.time));

        SpawnEvent firstEvent = events[0];

        while (eventIndex < events.Count)
        {
            SpawnEvent e = events[eventIndex];

            yield return new WaitForSeconds(eventIndex == 0 ? e.time : e.time - events[eventIndex - 1].time);

            if (e == firstEvent && currentWaveIndex > 0)
            {
                Debug.Log(
                    isFinalWave
                    ? "⚠️ FINAL WAVE!"
                    : "⚠️ HUGE WAVE!"
                );

                OnWaveWarning?.Invoke(isFinalWave);

                /*while (newWaveStart == false)
                {
                    yield return null;
                }*/
                OnWaveFirstSpawnEvent?.Invoke(currentWaveIndex);
            }

            int eventId = spawnEventIdCounter++;
            activeSpawnEvents.Add(eventId);
            StartCoroutine(SpawnEvent(e, eventId));

            eventIndex++;
            if (eventIndex == events.Count - 1)
            {
                newWaveStart = false;
            }
        }

        // Wait for all spawn events to complete before finishing the wave
        while (activeSpawnEvents.Count > 0)
        {
            yield return null;
        }
    }

    IEnumerator SpawnEvent(SpawnEvent e, int eventId)
    {
        int count = e.randomCount
            ? UnityEngine.Random.Range(e.countRange.x, e.countRange.y + 1)
            : e.fixedCount;

        for (int i = 0; i < count; i++)
        {
            EnemyData enemy = e.enemyPool[
                UnityEngine.Random.Range(0, e.enemyPool.Count)
            ];

            int lane = e.randomLane
                ? UnityEngine.Random.Range(e.minLane, e.maxLane + 1)
                : e.fixedLane;

            SpawnEnemy(enemy, lane);

            if (i == count - 1)
            {
                break;
            }

            yield return new WaitForSeconds(e.interval);
        }
        
        newWaveStart = true;
        activeSpawnEvents.Remove(eventId);
    }

    private void SpawnEnemy(EnemyData enemyData, int lane)
    {
        if (!GameManager.Instance.isLevelOnGoing)
        {
            return;
        }
        Vector3 spawnPosition = spawnPos[lane].position;
        GameObject go = ObjectPool.Instance.Spawn(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.Place(lane);
    }
}
