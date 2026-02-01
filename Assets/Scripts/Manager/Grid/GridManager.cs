using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private GridCell[] gridCells;
    public static int width = 10;
    public static int height = 5;
    private Dictionary<Tuple<int, int>, GridCell> gridCellsDict = new();

    [Header("References")]
    public Tilemap backgroundTilemap;
    public Transform gridHolder;
    public Transform boundaryHolder;

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

    public void LoadMapVisuals(MapThemeData theme)
    {
        if (theme == null)
            return;

        GenerateMap(WaveManager.Instance.levelData);

        bool isLightCell = true;
        foreach (Transform child in gridHolder)
        {
            var cell = child.GetComponent<GridCell>();
            if (cell != null)
            {
                // Cập nhật Sprite cho cell
                cell.UpdateTheme(theme, isLightCell);
                isLightCell = !isLightCell;
            }
        }

        foreach (Transform child in boundaryHolder)
        {
            switch (child.name)
            {
                case "TopLeftCorner":
                    SetSprite(child, theme.boundaryUpperLeftCorner);
                    break;
                case "BotLeftCorner":
                    SetSprite(child, theme.boundaryLowerLeftCorner);
                    break;
                case "BotRightCorner":
                    SetSprite(child, theme.boundaryLowerRightCorner);
                    break;
                case "TopRightCorner":
                    SetSprite(child, theme.boundaryUpperRightCorner);
                    break;

                case "Bot":
                    SetSpriteForChildren(child, theme.boundaryStraightLower);
                    break;
                case "Top":
                    SetSpriteForChildren(child, theme.boundaryStraightUpper);
                    break;
                case "Left":
                    SetSpriteForChildren(child, theme.boundaryStraightLeft);
                    break;
                case "Right":
                    SetSpriteForChildren(child, theme.boundaryStraightRight);
                    break;
            }
        }
    }

    public void GenerateMap(LevelData levelData)
    {
        backgroundTilemap.ClearAllTiles();

        foreach (var savedTile in levelData.mapLayout)
        {
            backgroundTilemap.SetTile(savedTile.position, savedTile.tileAsset);
        }
    }

    void SetSprite(Transform obj, Sprite sprite)
    {
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = sprite;
    }

    void SetSpriteForChildren(Transform parent, Sprite sprite)
    {
        foreach (Transform grandChild in parent)
        {
            var sr = grandChild.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = sprite;
        }
    }
}
