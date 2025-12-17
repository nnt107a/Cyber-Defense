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
    private bool newWaveStart = true;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UITextWarningEndWave.Instance.OnWaveWarningEnd += () =>
        {
            newWaveStart = true;
        };
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        for (currentWaveIndex = 0;
             currentWaveIndex < levelData.waves.Count;
             currentWaveIndex++)
        {
            newWaveStart = false;
            WaveEntry entry = levelData.waves[currentWaveIndex];
            bool isFinalWave =
                currentWaveIndex == levelData.waves.Count - 1;

            yield return StartCoroutine(RunWave(entry.wave, isFinalWave));

            while (newWaveStart == false)
            {
                yield return null;
            }

            yield return new WaitForSeconds(entry.intervalAfterWave);
        }
    }

    IEnumerator RunWave(WaveData wave, bool isFinalWave)
    {
        float timer = 0f;
        int eventIndex = 0;

        List<SpawnEvent> events =
            new List<SpawnEvent>(wave.spawnEvents);

        events.Sort((a, b) => a.time.CompareTo(b.time));

        SpawnEvent lastEvent = events[events.Count - 1];

        while (eventIndex < events.Count)
        {
            timer += Time.deltaTime;

            SpawnEvent e = events[eventIndex];

            if (timer >= e.time)
            {
                if (e == lastEvent)
                {
                    Debug.Log(
                        isFinalWave
                        ? "⚠️ FINAL WAVE!"
                        : "⚠️ HUGE WAVE!"
                    );

                    OnWaveWarning?.Invoke(isFinalWave);
                }

                StartCoroutine(SpawnEvent(e));
                eventIndex++;
            }

            yield return null;
        }
    }
    IEnumerator SpawnEvent(SpawnEvent e)
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

            yield return new WaitForSeconds(e.interval);
        }
    }
    private void SpawnEnemy(EnemyData enemyData, int lane)
    {
        Vector3 spawnPosition = spawnPos[lane].position;
        GameObject go = ObjectPool.Instance.Spawn(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.Place(lane);
    }
}
