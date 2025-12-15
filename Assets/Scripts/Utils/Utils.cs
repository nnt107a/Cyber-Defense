using UnityEngine;

public class Utils
{
    protected static LayerMask gridCellLayer = LayerMask.GetMask("Grid");
    public static GridCell GetGridCellAt(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos, gridCellLayer);
        return hit ? hit.GetComponent<GridCell>() : null;
    }
    public static bool IsWithinGridRadius(GridCell center, GridCell other, int radius)
    {
        int dx = Mathf.Abs(center.x - other.x);
        int dy = Mathf.Abs(center.y - other.y);

        return Mathf.Max(dx, dy) <= radius;
    }
    public static bool IsTopLane(GridCell cell)
    {
        return cell != null && cell.x >= 0 && cell.x < GridManager.width && cell.y >= 0 && cell.y < GridManager.height;
    }
    public static bool IsBottomLane(GridCell cell)
    {
        return cell != null && cell.x >= GridManager.width && cell.x < GridManager.width * 2 && cell.y >= 0 && cell.y < GridManager.height;
    }
}