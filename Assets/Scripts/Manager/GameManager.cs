using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<Enemy>[] enemiesInLane;
    public bool isLevelOnGoing = false;
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
    }/*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }*/
    public void Init()
    {
        isLevelOnGoing = false;
        for (int i = 0; i < GridManager.height; i++)
            enemiesInLane[i].Clear();
        LevelManager.Instance.Init();
    }
}
