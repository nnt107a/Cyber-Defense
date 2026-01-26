using TMPro;
using UnityEngine;

public class UIWikiList : MonoBehaviour
{
    public UIWikiElement[] elements;
    public UIWikiElement selectedElement = null;
    public GameObject detailsPanel;
    public TextMeshProUGUI descriptionText;
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
            descriptionText.text = enemyData.description;
        }
        else if (!isEnemyList && turretData != null)
        {
            descriptionText.text = turretData.description;
        }
    }
    public void SelectFaction(bool isTurret)
    {
        foreach (UIWikiElement element in elements)
        {
            bool shouldShow = isTurret ? element.turretData != null : element.enemyData != null;
            element.gameObject.SetActive(shouldShow);
        }
        selectedElement = null;
        detailsPanel.SetActive(false);
    }
}
