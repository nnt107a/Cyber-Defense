using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class UITextWarningEndWave : MonoBehaviour
{
    public static UITextWarningEndWave Instance;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private RectTransform rect;

    [Header("Timings")]
    public float moveUpTime = 0.6f; 
    public float stayTime = 1.2f;
    public float moveDownTime = 0.5f;

    private float anchorHeight;

    public Action OnWaveWarningEnd;

    void Awake()
    {
        Instance = this;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        anchorHeight = rect.anchorMax.y - rect.anchorMin.y;
    }
    private void Start()
    {
        WaveManager.Instance.OnWaveWarning += HandleWaveWarning;
    }
    private void HandleWaveWarning(bool isFinalWave)
    {
        if (isFinalWave)
        {
            warningText.text = "Final Wave Incoming~";
        }
        else
        {
            warningText.text = "A huge wave is coming!";
        }
        warningText.alpha = 1f;
        PlayWarningWave();
    }

    public void PlayWarningWave()
    {
        rect.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(
            rect.DOAnchorMin(
                    new Vector2(rect.anchorMin.x, 0.5f - anchorHeight / 2),
                    moveUpTime
                )
                .SetEase(Ease.OutBack)
        );

        seq.Join(
            rect.DOAnchorMax(
                    new Vector2(rect.anchorMax.x, 0.5f + anchorHeight / 2),
                    moveUpTime
                )
                .SetEase(Ease.OutBack)
        );

        seq.AppendInterval(stayTime);

        seq.Append(
            rect.DOAnchorMin(
                    new Vector2(rect.anchorMin.x, -0.2f - anchorHeight / 2),
                    moveDownTime
                )
                .SetEase(Ease.InCubic)
        );

        seq.Join(
            rect.DOAnchorMax(
                    new Vector2(rect.anchorMax.x, -0.2f + anchorHeight / 2),
                    moveDownTime
                )
                .SetEase(Ease.InCubic)
        );

        seq.Join(
            warningText.DOFade(0f, moveDownTime)
        );

        seq.OnComplete(() =>
        {
            OnWaveWarningEnd?.Invoke();
        });
    }
}
