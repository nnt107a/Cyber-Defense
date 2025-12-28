using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private GridCell[] gridCells;
    public static int width = 10;
    public static int height = 5;
    private Dictionary<Tuple<int, int>, GridCell> gridCellsDict = new();
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < gridCells.Length; i++)
        {
            gridCells[i].y = i % height;
            gridCells[i].x = i / height;
            gridCellsDict.Add(Tuple.Create(gridCells[i].x, gridCells[i].y), gridCells[i]);
        }
    }
    public GridCell GetCell(int x, int y)
    {
        return gridCellsDict[Tuple.Create(x, y)];
    }
    public void ClearCells()
    {
        foreach (var cell in gridCells)
        {
            if (cell.unitPlaced)
            {
                Destroy(cell.unit);
                cell.RemoveTurret();
            }
        }
    }
}
