using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWikiList : MonoBehaviour
{
    public UIWikiElement[] elements;
    public UIWikiElement selectedElement = null;
    public GameObject detailsPanel;
    public TextMeshProUGUI descriptionText;
    public Image elementIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI levelText;
    public bool isEnemyList = false;
    private void OnEnable()
    {
        selectedElement = null;
        foreach (var element in elements)
        {
            element.selectedOverlay.enabled = false;
        }
        SelectFaction(true);
    }
    public void SelectElement(UIWikiElement wikiElement, TurretData turretData, EnemyData enemyData)
    {
        foreach (var element in elements)
        {
            if (element != wikiElement)
                element.selectedOverlay.enabled = false;
        }

        wikiElement.selectedOverlay.enabled = true;
        selectedElement = wikiElement;
        detailsPanel.SetActive(true);
        isEnemyList = enemyData != null;
        if (isEnemyList && enemyData != null)
        {
            descriptionText.text = PlayerPrefs.GetInt("LocaleIndex", 0) == 0 ? enemyData.description : enemyData.vnDescription;
            elementIcon.sprite = enemyData.enemyIcon;
            nameText.text = enemyData.enemyName;
            healthText.text = enemyData.maxHealth.ToString();
            attackText.text = enemyData.attackDamage.ToString();
            attackSpeedText.text = enemyData.attackSpeed.ToString();
            speedText.text = enemyData.moveSpeed.ToString();
            speedText.transform.parent.gameObject.SetActive(true);
            costText.transform.parent.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            upgradeButton.onClick.RemoveAllListeners();
        }
        else if (!isEnemyList && turretData != null)
        {
            descriptionText.text = PlayerPrefs.GetInt("LocaleIndex", 0) == 0 ? turretData.description : turretData.vnDescription;
            elementIcon.sprite = turretData.turretIcon;
            nameText.text = turretData.turretName;
            Debug.Log("Health Text: " + healthText.text);
            Debug.Log("Turret health: " + turretData.maxHealth);
            Debug.Log("Turret ID: " + turretData.turretID);
            Debug.Log("Turret: " + GameManager.Instance.GetTurretDataByID(turretData.turretID));
            Debug.Log("Turret level: " + GameManager.Instance.GetTurretDataByID(turretData.turretID).level);
            healthText.text = (turretData.maxHealth + (GameManager.Instance.GetTurretDataByID(turretData.turretID).level - 1) * 5).ToString();
            attackText.text = turretData.attackDamage.ToString();
            attackSpeedText.text = turretData.attackSpeed.ToString();
            costText.text = turretData.eCoreCost.ToString();
            costText.transform.parent.gameObject.SetActive(true);
            speedText.transform.parent.gameObject.SetActive(false);
            levelText.gameObject.SetActive(true);
            levelText.text = "Lv " + GameManager.Instance.GetTurretDataByID(turretData.turretID).level.ToString();
            upgradeButton.gameObject.SetActive(true);
            upgradeCostText.text = turretData.eCoreCost.ToString();
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() =>
            {
                if (TechManager.Instance.ECrystalCount < turretData.eCoreCost)
                    return;
                TechManager.Instance.SpendECrystal(turretData.eCoreCost);
                //Upgrade turret and save
                GameManager.Instance.GetTurretDataByID(turretData.turretID).UpgradeLevel();
                levelText.text = "Lv " + GameManager.Instance.GetTurretDataByID(turretData.turretID).level.ToString();

                healthText.text = (turretData.maxHealth + (GameManager.Instance.GetTurretDataByID(turretData.turretID).level - 1) * 5).ToString();

                GameManager.Instance.SaveToDisk();
            });
        }
    }
    public void SelectFaction(bool isTurret)
    {
        foreach (UIWikiElement element in elements)
        {
            bool shouldShow = isTurret ? element.turretData != null : element.enemyData != null;
            element.gameObject.SetActive(shouldShow);
            if (isTurret && element.turretData != null)
            {
                bool unlocked = GameManager.Instance.GetTurretUnlockedStatus(element.turretData.turretID);
                element.transform.Find("Image").gameObject.SetActive(!unlocked);
                element.GetComponent<Button>().interactable = unlocked;
            }
            element.selectedOverlay.enabled = false;

        }
        selectedElement = null;
        detailsPanel.SetActive(false);
    }
}
