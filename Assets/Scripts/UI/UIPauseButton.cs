using UnityEngine;
using UnityEngine.UI;

public class UIPauseButton : MonoBehaviour
{
    [SerializeField] private Button button;
    private void Start()
    {
        button.onClick.AddListener(GameManager.Instance.Pause);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(GameManager.Instance.Pause);
    }
}
