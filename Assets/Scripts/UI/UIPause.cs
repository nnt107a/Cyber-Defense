using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    private void Start()
    {
        GameManager.Instance.OnPause += Show;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= Show;
    }
    private void Show()
    {
        panel.gameObject.SetActive(true);
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.5f).SetUpdate(true);
    }
    public void GoHome()
    {

    }
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Continue()
    {
        panel.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            panel.gameObject.SetActive(false);
            panel.interactable = false;
            panel.blocksRaycasts = false;
        });
    }
}
