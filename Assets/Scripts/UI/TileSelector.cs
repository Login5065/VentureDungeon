using System.Collections.Generic;
using UnityEngine;
using Dungeon.MapSystem;
using Dungeon.Variables;
using System;

namespace Dungeon.UI
{
    public class TileSelector : MonoBehaviour
    {
        private RectInt? bounds;
        public List<ExtendedTile> selectedTiles;
        public List<Vector3Int> selectedCoords;

        private void Start()
        {
            Statics.SelectionBox.OnSelectionFinished += SelectionFinished;
        }

        private void SelectionFinished(RectInt selection)
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.Build && !MouseInputUIBlocker.BlockedByUI)
            {
                if (selection.size.x > 0 && selection.size.y > 0)
                    bounds = selection;
                else
                {
                    var coords = Statics.TileMapFG.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    selectedCoords.Add(coords);
                    selectedTiles.Add(Statics.TileMapFG.GetTile<ExtendedTile>(coords));
                }
                ExecuteOperations();
                ClearSelection();
            }
        }

        public void Build(int index)
        {
            foreach (Vector3Int tile in selectedCoords)
            {
                Statics.MapManager.ReplaceTile(Statics.ArchitectUIManager.ground, tile, newData: TileDictionary.TileData[Enum.GetName(typeof(ArchitectUIManager.Architect), index).ToLower()]);
            }
        }

        public void Mine()
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                Statics.MapManager.BreakTile(Statics.ArchitectUIManager.ground, selectedCoords[i]);
            }
        }

        public void ExecuteOperations()
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.Build)
            {
                if (bounds.HasValue)
                    GetAllTiles(bounds.Value);
                if (Statics.ArchitectUIManager.material == (int)ArchitectUIManager.Architect.Mine)
                {
                    Mine();
                }
                else
                {
                    Build(Statics.ArchitectUIManager.material);
                }
            }
        }

        public void ClearSelection()
        {
            bounds = null;
            selectedTiles.Clear();
            selectedCoords.Clear();
        }

        private void GetAllTiles(RectInt bounds)
        {
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (int j = bounds.yMin; j < bounds.yMax; j++)
                {
                    selectedTiles.Add(Statics.TileMapFG.GetTile<ExtendedTile>(new Vector3Int(i, j, 0)));
                    selectedCoords.Add(new Vector3Int(i, j, 0));
                }
            }
        }
    }
}