using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUIButtonHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        GameManager.Instance.OnLevelCompleted += Show;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnLevelCompleted -= Show;
    }
    private void Show()
    {
        if (GameManager.Instance.currentLevelIndex
            >= GameManager.Instance.allLevelDatas.Length - 1)
        {
            continueButton.SetActive(false);
        }
        panel.gameObject.SetActive(true);
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
        GameManager.Instance.currentLevelIndex = Mathf.Min(GameManager.Instance.currentLevelIndex + 1,
            GameManager.Instance.allLevelDatas.Length - 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
