using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroFlow : MonoBehaviour
{
    public static LevelIntroFlow Instance;
    [SerializeField] private CameraIntroController cameraController;
    [SerializeField] private LoadoutBoardController loadoutBoard;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject shopContainer; 
    [SerializeField] LoadoutEnemyPreviewSpawner previewSpawner;

    [SerializeField] private CutscenePlayer cutscenePlayer;
    [SerializeField] private CutsceneData introCutsceneData;
    [SerializeField] private CutsceneData outroCutsceneData;
    public Action OnLevelCompleted;

    [SerializeField] private float cameraMoveTime = 1.2f;

    bool waitingForStart;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GameManager.Instance.OnLevelCompleted += HandleLevelCompleted;
        GameManager.Instance.isLevelOnGoing = false;
        WaveManager.Instance.levelData = GameManager.Instance.allLevelDatas[GameManager.Instance.currentLevelIndex];
        if (introCutsceneData != null)
        {
            cutscenePlayer.onCutsceneFinished.RemoveAllListeners();
            cutscenePlayer.onCutsceneFinished.AddListener(() =>
            {
                StartCoroutine(IntroSequence());
            });
            cutscenePlayer.Play(introCutsceneData);
        }
        else
        {
            StartCoroutine(IntroSequence());
        }
    }

    IEnumerator IntroSequence()
    {
        gameplayUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        previewSpawner.SpawnPreviews();

        yield return cameraController.MoveToEnemy(cameraMoveTime).WaitForCompletion();

        loadoutBoard.Show();
        yield return new WaitForSeconds(0.4f);

        startButton.SetActive(true);
        startButton.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).From(0f);

        shopContainer.SetActive(true);
        shopContainer.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).From(0f);

        waitingForStart = true;
        while (waitingForStart)
            yield return null;

        GameManager.Instance.isTransitioningAfterChoosingLoadout = true;

        startButton.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            startButton.SetActive(false);
        });

        loadoutBoard.Hide();
        yield return new WaitForSeconds(0.6f);

        yield return cameraController.MoveToYard(cameraMoveTime).WaitForCompletion();

        GameManager.Instance.isTransitioningAfterChoosingLoadout = false;

        StartGameplay();
    }

    public void OnStartButtonClicked()
    {
        waitingForStart = false;
    }

    private void StartGameplay()
    {
        previewSpawner.Clear();
        GameManager.Instance.enemiesSpawnedCompletely = false;
        GameManager.Instance.isLevelOnGoing = true;
        Debug.Log("Gameplay started! Level " + GameManager.Instance.currentLevelIndex);
        LoadoutManager.Instance.RefreshShopBar();
        gameplayUI.SetActive(true);
        gameplayUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).From(0f);
        GameManager.Instance.Init();
        WaveManager.Instance.StartWaveSpawn();
    }
    private void HandleLevelCompleted()
    {
        if (outroCutsceneData != null)
        {
            cutscenePlayer.onCutsceneFinished.RemoveAllListeners();
            cutscenePlayer.onCutsceneFinished.AddListener(() =>
            {
                OnLevelCompleted?.Invoke();
            });
            cutscenePlayer.Play(outroCutsceneData);
        }
        else
        {
            OnLevelCompleted?.Invoke();
        }
    }
}
