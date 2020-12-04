using Dungeon.MapSystem;
using Dungeon.Variables;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (target is MapManager mapManager && Statics.TileDictionary != null)
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
        }
    }
}