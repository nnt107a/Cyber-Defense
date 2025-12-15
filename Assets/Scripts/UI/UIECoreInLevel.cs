using TMPro;
using UnityEngine;

public class UIECoreInLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eCoreValue;
    private void Start()
    {
        LevelManager.Instance.OnECoreCountChanged += UpdateECoreCount;
        UpdateECoreCount(LevelManager.Instance.eCoreCount);
    }
    private void OnDestroy()
    {
        LevelManager.Instance.OnECoreCountChanged -= UpdateECoreCount;
    }
    private void UpdateECoreCount(int newCount)
    {
        eCoreValue.text = newCount.ToString();
    }
}
