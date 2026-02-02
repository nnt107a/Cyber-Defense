using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSlot : MonoBehaviour
{
    public TurretData turretData;
    public Image icon;
    public TextMeshProUGUI eCoreCost;
    public Image selectedOverlay;
    public TextMeshProUGUI turretName;

    void Start()
    {
        icon.sprite =
            turretData.turretIcon;
        eCoreCost.text = turretData.eCoreCost.ToString();
        turretName.text = turretData.turretName;

        GetComponent<Button>().onClick.AddListener(OnClick);
        Refresh();
    }

    void OnClick()
    {
        if (LoadoutManager.Instance.IsSelected(turretData))
            LoadoutManager.Instance.UnselectTurret(turretData);
        else
            LoadoutManager.Instance.SelectTurret(turretData);

        Refresh();

        TutorialManager.Instance.OnClick_DisableTutorial();
    }

    public void Refresh()
    {
        bool unlocked = GameManager.Instance.GetTurretUnlockedStatus(turretData.turretID);
        transform.Find("Image").gameObject.SetActive(!unlocked);
        GetComponent<Button>().interactable = unlocked;
        selectedOverlay.enabled =
            LoadoutManager.Instance.IsSelected(turretData);
    }
}
