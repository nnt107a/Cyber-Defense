using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWikiElement : MonoBehaviour
{
    public UIWikiList uIWikiList;
    public TurretData turretData;
    public EnemyData enemyData;
    public Image icon;
    public Image selectedOverlay;

    void Start()
    {
        icon.sprite =
            enemyData != null ? enemyData.enemyIcon : turretData.turretIcon;
        GetComponentInChildren<TextMeshProUGUI>().text = 
            enemyData != null ? enemyData.enemyName : turretData.turretName;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uIWikiList.SelectElement(this, turretData, enemyData);
    }
}
