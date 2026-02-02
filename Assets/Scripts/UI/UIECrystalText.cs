using TMPro;
using UnityEngine;

public class UIECrystalText : MonoBehaviour
{
    public TextMeshProUGUI eCrystalText;

    void Awake()
    {
        UpdateECrystalCount(TechManager.Instance.ECrystalCount);
    }

    void Start()
    {
        if (TechManager.Instance != null)
        {
            TechManager.Instance.OnECrystalChanged += UpdateECrystalCount;
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        if (TechManager.Instance != null)
        {
            TechManager.Instance.OnECrystalChanged -= UpdateECrystalCount;
        }
    }

    public void UpdateECrystalCount(int ECrystalCount)
    {
        if (eCrystalText != null)
            eCrystalText.text = ECrystalCount.ToString();
    }
}
