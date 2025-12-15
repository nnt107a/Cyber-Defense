using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x;
    public int y;
    public bool unitPlaced = false;
    public GameObject unit;
    public void OnMouseDown()
    {
        if (HandleClick())
        {
            UnitDragHandler.Instance.dragging = false;
            Destroy(UnitDragHandler.Instance.dragPreview);
        }
    }
    private bool HandleClick()
    {
        if (unitPlaced && !UnitDragHandler.Instance.isSellAction)
        {
            return false;
        }
        else if (!unitPlaced && UnitDragHandler.Instance.isSellAction)
        {
            return false;
        }
        if (UnitDragHandler.Instance.isSellAction)
        {
            LevelManager.Instance.ChangeECoreCount((int)(unit.GetComponent<Turret>().turretData.eCoreCost * 0.7f));
            Destroy(unit);
            unit = null;
            unitPlaced = false;
            return true;
        }
        unit = Instantiate(UnitDragHandler.Instance.shopElement.GetComponent<ShopElement>().towerPrefab, transform);
        unit.GetComponent<Turret>().Place(y);
        unit.transform.localPosition = Vector3.zero;
        unitPlaced = true;
        LevelManager.Instance.ChangeECoreCount(-unit.GetComponent<Turret>().turretData.eCoreCost);
        return true;
    }
}
