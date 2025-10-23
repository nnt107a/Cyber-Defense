using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x;
    public int y;
    public bool unitPlaced = false;
    public GameObject unit;
    public void OnMouseDown()
    {
        Debug.Log("Gone here");
        if (PlaceTower())
        {
            UnitDragHandler.Instance.dragging = false;
            Destroy(UnitDragHandler.Instance.dragPreview);
        }
    }
    private bool PlaceTower()
    {
        if (unitPlaced)
        {
            return false;
        }
        unit = Instantiate(UnitDragHandler.Instance.shopElement.GetComponent<ShopElement>().towerPrefab, transform);
        unit.transform.localPosition = Vector3.zero;
        unitPlaced = true;
        return true;
    }
}
