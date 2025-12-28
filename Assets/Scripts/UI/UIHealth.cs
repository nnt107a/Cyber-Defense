using TMPro;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthAmount;
    private void Start()
    {
        UpdateHealthUI(GameManager.Instance.maxHealth);
        GameManager.Instance.OnHealthChanged += UpdateHealthUI;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnHealthChanged -= UpdateHealthUI;
    }
    private void UpdateHealthUI(int newHealth)
    {
        healthAmount.text = newHealth.ToString();
    }
}
