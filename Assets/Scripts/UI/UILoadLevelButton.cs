using DG.Tweening;
using UnityEngine;

public class UILoadLevelButton : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName="SampleScene";
    [SerializeField]
    private GameObject levelIndicator;

    public void Show()
    {

        transform.localScale = Vector3.one;

        if (levelIndicator != null)
        {
            levelIndicator.SetActive(false);
        }

        transform
            .DOScale(1.2f, 0.3f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                ShowIndicator();
            });
    }

    private void ShowIndicator()
    {
        if (levelIndicator != null)
        {
            levelIndicator.SetActive(true);
            levelIndicator.transform.localScale = Vector3.zero;
            levelIndicator.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
    }

    public void loadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    private void OnDisable()
    {
        if (levelIndicator != null)
        {
            levelIndicator.transform.DOKill();
        }
        transform.DOKill();
    }
}
