using Dungeon.MapSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (target is MapManager mapManager)
        {
            // Save map
            if (GUILayout.Button("Save map"))
                mapManager.SaveMap();

            // Load map
            if (GUILayout.Button("Load map"))
            {
                var file = EditorUtility.OpenFilePanel("Select map", "", "json");
                if (!string.IsNullOrWhiteSpace(file))
                    mapManager.LoadMap(file);
            }

            // Load tiles data and combine with existing ones (overwrite if any conflicts)
            if (GUILayout.Button("Load tiles data"))
            {
                var file = EditorUtility.OpenFilePanel("Select tile data", "", "json");
                if (!string.IsNullOrWhiteSpace(file))
                    TileDictionary.LoadTileData(file, true);
            }

            // Save or clear tiles data, but only if any are loaded
            if (TileDictionary.TileData.Count <= 0)
                EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Save tiles data"))
                TileDictionary.SaveFileData("base_tiles_regenerated.json", true);
            if (GUILayout.Button("Clear tiles data"))
                TileDictionary.TileData.Clear();
            if (TileDictionary.TileData.Count <= 0)
                EditorGUI.EndDisabledGroup();
        }
    }
}