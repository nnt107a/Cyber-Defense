using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUIButtonHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private bool isWin = false;
    private void Start()
    {
        LevelIntroFlow.Instance.OnLevelCompleted += ShowOnWin;
        GameManager.Instance.OnLevelLosed += ShowOnLose;
    }
    private void OnDestroy()
    {
        LevelIntroFlow.Instance.OnLevelCompleted -= ShowOnWin;
        GameManager.Instance.OnLevelLosed -= ShowOnLose;
    }
    private void ShowOnWin()
    {
        if (!isWin)
            return;
        if (GameManager.Instance.currentLevelIndex
            >= GameManager.Instance.allLevelDatas.Length - 1)
        {
            continueButton.SetActive(false);
        }
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f);
    }
    private void ShowOnLose()
    {
        if (isWin)
            return;
        if (GameManager.Instance.currentLevelIndex
            >= GameManager.Instance.allLevelDatas.Length - 1)
        {
            continueButton.SetActive(false);
        }
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f);
    }
    public void GoHome()
    {

    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        // 1. Gọi GameManager để mở khóa level tiếp theo
        GameManager.Instance.UnlockNextLevel();

        // 2. Chuyển về màn hình chọn level (LevelSelect)
        // Đảm bảo tên Scene trong Build Settings là "LevelSelect" (hoặc tên bạn đặt)
        SceneManager.LoadScene("LevelSelect");
    }
}
