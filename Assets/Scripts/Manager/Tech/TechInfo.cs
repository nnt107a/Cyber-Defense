using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechInfo : MonoBehaviour
{
    private TechData techData;
    [SerializeField] private TextMeshProUGUI techNameText;
    [SerializeField] private TextMeshProUGUI techDescriptionText;
    [SerializeField] private GameObject techCost;
    [SerializeField] private GameObject unlockButton;

    void Start()
    {
        TechManager.Instance.OnTechUnlocked += (int newECrystalCount) => UpdateECrystalValidity(newECrystalCount);

        unlockButton.GetComponent<Button>()
            .onClick.AddListener(() => 
            {
                if (TechManager.Instance.TryResearch(techData))
                {
                    techCost.SetActive(false);
                    unlockButton.SetActive(false);
                }
            });
    }

    public void SetUp(TechData techData)
    {
        this.techData = techData;
        techNameText.text = techData.TechName;
        techDescriptionText.text = PlayerPrefs.GetInt("LocaleIndex", 0) == 0 ? techData.TechDescription : techData.TechVNDescription;
        unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        UpdateECrystalValidity(TechManager.Instance.ECrystalCount);

        if (TechManager.Instance.IsUnlocked(techData))
        {
            unlockButton.SetActive(false);
            techCost.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(true);
            techCost.SetActive(true);
            techCost.GetComponentInChildren<TextMeshProUGUI>().text = techData.eCrystalRequired.ToString();
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
            techCost.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            techCost.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
    }
}
