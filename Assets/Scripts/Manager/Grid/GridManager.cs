using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridCell[] gridCells;
    private int width = 10;
    private int height = 5;
    private Dictionary<Tuple<int, int>, GridCell> gridCellsDict;
    private void Awake()
    {
        for (int i = 0; i < gridCells.Length; i++)
        {
            gridCells[i].x = i % width;
            gridCells[i].y = i / width;
        }
    }
    public GridCell GetCell(int x, int y)
    {
        return gridCellsDict[Tuple.Create(x, y)];
    }
}
