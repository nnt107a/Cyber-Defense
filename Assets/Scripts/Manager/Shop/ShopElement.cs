using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    public GameObject towerPrefab;
    [SerializeField] private TextMeshProUGUI towerCostText;

    private void Start()
    {
        GetComponent<Image>().sprite = towerPrefab.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Button>().onClick.AddListener(() => 
        { 
            if (LevelManager.Instance.eCoreCount < towerPrefab.GetComponent<Turret>().turretData.eCoreCost)
            {
                return;
            }
            if (UnitDragHandler.Instance.dragPreview != null && UnitDragHandler.Instance.dragPreview.gameObject.activeInHierarchy)
            {
                Destroy(UnitDragHandler.Instance.dragPreview);
            }
            UnitDragHandler.Instance.BeginDrag(gameObject); 
        });
        towerCostText.text = towerPrefab.GetComponent<Turret>().turretData.eCoreCost.ToString();
    }
}
