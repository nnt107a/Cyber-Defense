using DG.Tweening;
using UnityEngine;

public class LoadoutBoardController : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;

    private float anchorHeight;

    void Awake()
    {
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;

        anchorHeight = panel.anchorMax.y - panel.anchorMin.y;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        seq.Append(
            panel.DOAnchorMin(
                    new Vector2(panel.anchorMin.x, 0.4f - anchorHeight / 2), 
                    0.4f
                )
                .SetEase(Ease.OutBack)
        );

        seq.Join(
            panel.DOAnchorMax(
                    new Vector2(panel.anchorMax.x, 0.4f + anchorHeight / 2),
                    0.4f
                )
                .SetEase(Ease.OutBack)
        );

        canvasGroup.DOFade(1f, 0.4f).OnComplete(() =>
        {
            TutorialManager.Instance.OnClick_TutorialStep((int)TutorialSteps.EnterLevel);
        });
    }

    public void Hide()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            panel.DOAnchorMin(
                    new Vector2(panel.anchorMin.x, -0.6f - anchorHeight / 2),
                    0.6f
                )
                .SetEase(Ease.OutBack)
        );

        seq.Join(
            panel.DOAnchorMax(
                    new Vector2(panel.anchorMax.x, -0.6f + anchorHeight / 2),
                    0.6f
                )
                .SetEase(Ease.OutBack)
        );

        canvasGroup.DOFade(0f, 0.6f);

        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
