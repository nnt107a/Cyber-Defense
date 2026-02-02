#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelData data = (LevelData)target;

        GUILayout.Space(20);
        GUILayout.Label("LEVEL DESIGN TOOLS", EditorStyles.boldLabel);

        if (GUILayout.Button("Save Current Tilemap to Data"))
        {
            SaveMap(data);
        }

        GUILayout.Label($"Stored Tiles: {data.mapLayout.Count}");
    }

    private void SaveMap(LevelData data)
    {
        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found in the scene.");
            return;
        }

        data.mapLayout.Clear();

        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(pos);

                if (tile != null)
                {
                    data.mapLayout.Add(new SavedTile { position = pos, tileAsset = tile });
                }
            }
        }

        EditorUtility.SetDirty(data);
        Debug.Log($"✅ Saved {data.mapLayout.Count} tiles to LevelData: {data.name}");
    }
}
#endif
