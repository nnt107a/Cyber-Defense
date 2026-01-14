using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechInfo : MonoBehaviour
{
    private TechData techData;
    [SerializeField] private TextMeshProUGUI techNameText;
    [SerializeField] private TextMeshProUGUI techDescriptionText;
    [SerializeField] private TextMeshProUGUI techCostText;
    [SerializeField] private GameObject unlockButton;

    void Start()
    {
        TechManager.Instance.OnTechUnlocked += (int newECrystalCount) => UpdateECrystalValidity(newECrystalCount);

        unlockButton.GetComponent<Button>()
            .onClick.AddListener(() => 
            {
                if (TechManager.Instance.TryResearch(techData))
                {
                    techCostText.GetComponentInParent<Transform>().gameObject.SetActive(false);
                    unlockButton.SetActive(false);
                }
            });
    }

    public void SetUp(TechData techData)
    {
        this.techData = techData;
        techNameText.text = techData.TechName;
        techDescriptionText.text = techData.TechDescription;
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        UpdateECrystalValidity(TechManager.Instance.ECrystalCount);

        if (TechManager.Instance.IsUnlocked(techData))
        {
            unlockButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
            if (TechManager.Instance.CanUnlock(techData))
            {
                unlockButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                unlockButton.GetComponent<Button>().interactable = false;
                unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
        }
    }

    private void UpdateECrystalValidity(int newECrystalCount)
    {
        if (TechManager.Instance.IsEnoughECrystal(techData))
        {
            techCostText.color = Color.white;
        }
        else
        {
            techCostText.color = Color.red;
        }
    }
}
