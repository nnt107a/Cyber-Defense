using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveData currentWave;

    [SerializeField] private Transform[] spawnPos;
    private float timer;
    private int eventIndex;

    void Update()
    {
        timer += Time.deltaTime;

        while (eventIndex < currentWave.spawnEvents.Count &&
               timer >= currentWave.spawnEvents[eventIndex].time)
        {
            StartCoroutine(SpawnEvent(currentWave.spawnEvents[eventIndex]));
            eventIndex++;
        }
    }

    IEnumerator SpawnEvent(SpawnEvent e)
    {
        int count = e.randomCount
            ? Random.Range(e.countRange.x, e.countRange.y + 1)
            : e.fixedCount;

        for (int i = 0; i < count; i++)
        {
            EnemyData enemy = e.enemyPool[
                Random.Range(0, e.enemyPool.Count)
            ];

            int lane = e.randomLane
                ? Random.Range(e.minLane, e.maxLane + 1)
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
