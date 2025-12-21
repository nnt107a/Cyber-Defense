using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<Enemy>[] enemiesInLane;
    public bool isLevelOnGoing = false;
    public LevelData[] allLevelDatas;
    public int currentLevelIndex = 0;

    public Action OnLevelCompleted;

    public bool enemiesSpawnedCompletely = false;
    private void Awake()
    {
        enemiesInLane = new HashSet<Enemy>[GridManager.height];
        for (int i = 0; i < GridManager.height; i++)
            enemiesInLane[i] = new HashSet<Enemy>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if ((Time.frameCount & 31) == 0)
        {
            Debug.Log("Enemies counter: " + EnemiesCount() + ", Spawned completely: " + enemiesSpawnedCompletely);
            if (enemiesSpawnedCompletely && !EnemiesRemaining() && isLevelOnGoing)
            {
                isLevelOnGoing = false;
                StartCoroutine(DelayALittle());
            }
        }
    }
    private IEnumerator DelayALittle()
    {
        yield return new WaitForSeconds(1f);
        OnLevelCompleted?.Invoke();
    }
    private bool EnemiesRemaining()
    {
        for (int i = 0; i < GridManager.height; i++)
        {
            if (enemiesInLane[i].Count > 0)
                return true;
        }
        return false;
    }
    private int EnemiesCount()
    {
        int count = 0;
        for (int i = 0; i < GridManager.height; i++)
        {
            count += enemiesInLane[i].Count;
        }
        return count;
    }
    public void Init()
    {
        for (int i = 0; i < GridManager.height; i++)
            enemiesInLane[i].Clear();
        LevelManager.Instance.Init();
    }
}
