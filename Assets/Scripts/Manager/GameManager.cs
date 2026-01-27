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
    public int maxHealth = 5;
    private int health = 5;

    public Action OnLevelCompleted;
    public Action OnLevelLosed;
    public Action<int> OnHealthChanged;
    public Action OnPause;
    public Action OnGoToSetting;

    public bool enemiesSpawnedCompletely = false;
    public bool isTransitioningAfterChoosingLoadout = false;

    public TurretData[] turretDatas;

    public SaveData currentData;
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

        currentData = BinarySaveSystem.Load();

        Debug.Log("Data loaded: E-Crystal = " + currentData.eCrystal + ", Unlocked Techs = " + currentData.unlockedTechs.Count + ", Current turret data: " + currentData.turretSaveDatas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if ((Time.frameCount & 31) == 0)
        {
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
        health = 5;
        for (int i = 0; i < GridManager.height; i++)
            enemiesInLane[i].Clear();
        GridManager.Instance.ClearCells();
        LevelManager.Instance.Init();
    }
    public void ChangeHealth(int amount)
    {
        health = Mathf.Max(health + amount, 0);
        OnHealthChanged?.Invoke(health);

        StartCoroutine(DelayBeforeLose());
    }
    private IEnumerator DelayBeforeLose()
    {
        yield return new WaitForSeconds(1f);

        if (health <= 0)
        {
            isLevelOnGoing = false;
            OnLevelLosed?.Invoke();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }
    public void GoToSetting()
    {
        OnGoToSetting?.Invoke();
    }
    public void ShowWinUI()
    {
        OnLevelCompleted?.Invoke();
    }

    public void SaveToDisk()
    {/*
        currentData.turretSaveDatas = TurretManager.Instance.turretDatas;*/
        BinarySaveSystem.Save(currentData);
        Debug.Log("Save data to disk!");
    }

    public void ApplyDataToManagers()
    {
        TechManager.Instance.SyncData(currentData.unlockedTechs, currentData.eCrystal);
        TurretManager.Instance.SyncData(currentData.turretSaveDatas);

        // LevelManager.Instance.currentLevelIndex = data.currentLevelIndex;
    }
    public TurretSaveData GetTurretDataByID(int turretID)
    {
        return currentData.turretSaveDatas.Find(t => t.turretID == turretID);
    }
}
