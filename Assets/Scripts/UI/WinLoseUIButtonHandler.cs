using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUIButtonHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private bool isWin = false;
    [SerializeField] private TextMeshProUGUI crystalRewardText;
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
        TechManager.Instance.SpendECrystal(-RewardManager.Instance.GetCrystalReward(true));
        if (GameManager.Instance.currentLevelIndex
            >= GameManager.Instance.allLevelDatas.Length - 1)
        {
            continueButton.SetActive(false);
        }
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f); 
        crystalRewardText.text = RewardManager.Instance.GetCrystalReward(true).ToString();
    }
    private void ShowOnLose()
    {
        if (isWin)
            return;
        TechManager.Instance.SpendECrystal(-RewardManager.Instance.GetCrystalReward(false));
        if (GameManager.Instance.currentLevelIndex
            >= GameManager.Instance.allLevelDatas.Length - 1)
        {
            continueButton.SetActive(false);
        }
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f);
        crystalRewardText.text = RewardManager.Instance.GetCrystalReward(false).ToString();
    }
    public void GoHome()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        // Chuyển về màn hình chọn level (LevelSelect)
        // Đảm bảo tên Scene trong Build Settings là "LevelSelect" (hoặc tên bạn đặt)
        GameManager.Instance.currentLevelIndex++;
        SceneManager.LoadScene("SampleScene");
    }
}
