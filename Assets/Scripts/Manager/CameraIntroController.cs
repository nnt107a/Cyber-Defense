using DG.Tweening;
using UnityEngine;

public class CameraIntroController : MonoBehaviour
{
    [SerializeField] private Transform yardView;
    [SerializeField] private Transform enemyView;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    public Tween MoveToYard(float time)
    {
        return cam.transform.DOMove(yardView.position, time)
            .SetEase(Ease.InOutSine);
    }

    public Tween MoveToEnemy(float time)
    {
        return cam.transform.DOMove(enemyView.position, time)
            .SetEase(Ease.InOutSine);
    }
}
