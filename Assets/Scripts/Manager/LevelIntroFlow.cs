using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroFlow : MonoBehaviour
{
    [SerializeField] private CameraIntroController cameraController;
    [SerializeField] private LoadoutBoardController loadoutBoard;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject shopContainer;

    [SerializeField] private float cameraMoveTime = 1.2f;

    bool waitingForStart;
    void Start()
    {
        GameManager.Instance.isLevelOnGoing = false;
        WaveManager.Instance.levelData = GameManager.Instance.allLevelDatas[GameManager.Instance.currentLevelIndex];
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        gameplayUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);

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

    void StartGameplay()
    {
        GameManager.Instance.enemiesSpawnedCompletely = false;
        GameManager.Instance.isLevelOnGoing = true;
        Debug.Log("Gameplay started! Level " + GameManager.Instance.currentLevelIndex);
        LoadoutManager.Instance.RefreshShopBar();
        gameplayUI.SetActive(true);
        gameplayUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).From(0f);
        GameManager.Instance.Init();
        WaveManager.Instance.StartWaveSpawn();
    }
}
