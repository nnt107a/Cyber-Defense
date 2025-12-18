using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x;
    public int y;
    public bool unitPlaced = false;
    public GameObject unit;
    public void OnMouseDown()
    {
        Debug.Log("CLICKED GRID CELL AT: " + x + ", " + y);
        if (HandleClick())
        {
            UnitDragHandler.Instance.dragging = false;
            Destroy(UnitDragHandler.Instance.dragPreview);
        }
    }
    private bool HandleClick()
    {
        if (UnitDragHandler.Instance.shopElement == null && !UnitDragHandler.Instance.isSellAction)
        {
            return false;
        }
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
            unit.GetComponent<Turret>().ShowFloatingText("+" + ((int)(unit.GetComponent<Turret>().turretData.eCoreCost * 0.7f)).ToString(), Color.gold);
            LevelManager.Instance.ChangeECoreCount((int)(unit.GetComponent<Turret>().turretData.eCoreCost * 0.7f));
            Destroy(unit);
            unit = null;
            unitPlaced = false;
            return true;
        }
        unit = Instantiate(UnitDragHandler.Instance.shopElement.GetComponent<ShopElement>().towerPrefab, transform);
        Turret turret = unit.GetComponent<Turret>();
        turret.Place(y, this);
        unit.transform.localPosition = Vector3.zero;
        unitPlaced = true;
        turret.ShowFloatingText("-" + turret.turretData.eCoreCost.ToString(), Color.red);
        LevelManager.Instance.ChangeECoreCount(-unit.GetComponent<Turret>().turretData.eCoreCost);
        UnitDragHandler.Instance.shopElement.GetComponent<ShopElement>().Recharge();
        UnitDragHandler.Instance.shopElement = null;
        return true;
    }
}
