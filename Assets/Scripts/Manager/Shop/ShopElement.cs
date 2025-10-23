using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    public GameObject towerPrefab;
    public int cost;

    private void Start()
    {
        GetComponent<Image>().sprite = towerPrefab.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Button>().onClick.AddListener(() => UnitDragHandler.Instance.BeginDrag(this));
    }
}
