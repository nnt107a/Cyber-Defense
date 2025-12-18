using DG.Tweening;
using UnityEngine;

public class UILevelWaveMarker : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] RectTransform icon;

    float inactiveY = 0f;
    float activeY = 20f;

    public void Setup(float normalizedTime, bool isFinal)
    {
        rect.anchorMin = new Vector2(normalizedTime, 0.5f);
        rect.anchorMax = new Vector2(normalizedTime, 0.5f);
        rect.anchoredPosition = Vector2.zero;

        icon.anchoredPosition = new Vector2(0, inactiveY);

        if (isFinal)
            icon.localScale = Vector3.one * 1.3f;
    }

    public void Activate()
    {
        icon.DOAnchorPosY(activeY, 0.35f)
            .SetEase(Ease.OutBack);
    }
}
