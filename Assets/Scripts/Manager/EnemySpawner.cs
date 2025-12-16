using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnEnemiesPrefab;
    float spawnInterval = 5f;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        if (spawnEnemiesPrefab.Length == 0)
        {
            return;
        }
        GameObject monsterToSpawn = spawnEnemiesPrefab[Random.Range(0, spawnEnemiesPrefab.Length)];
        ObjectPool.Instance.Spawn(monsterToSpawn, transform.position, Quaternion.identity);
    }
}
