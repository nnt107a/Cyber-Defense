using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UILevelIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float moveDistance = 15f;

    [SerializeField]
    private float duration = 0.8f;

    private Vector3 startPos;
    private bool isInitialized = false;

    private void Awake()
    {
        startPos = transform.localPosition;
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized)
            startPos = transform.localPosition;

        StartCoroutine(StartFloating());
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public IEnumerator StartFloating()
    {
        yield return new WaitForSeconds(0.3f);
        transform.DOKill();
        transform.localPosition = startPos;

        transform.localScale = Vector3.one;

        transform
            .DOLocalMoveY(startPos.y + moveDistance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
