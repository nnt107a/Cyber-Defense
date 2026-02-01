using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapThemeData", menuName = "ScriptableObjects/MapThemeData")]
public class MapThemeData : ScriptableObject
{
    [Header("General Settings")]
    public string themeName;

    [Header("Tilemap Background")]
    public TileBase backgroundTile;

    [Header("Grid Cells")]
    public Sprite lightCell;
    public Sprite darkCell;

    [Header("Boundaries")]
    public Sprite boundaryUpperLeftCorner;
    public Sprite boundaryLowerLeftCorner;
    public Sprite boundaryUpperRightCorner;
    public Sprite boundaryLowerRightCorner;

    public Sprite boundaryStraightUpper;
    public Sprite boundaryStraightLower;
    public Sprite boundaryStraightLeft;
    public Sprite boundaryStraightRight;
}
