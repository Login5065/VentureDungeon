using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.Variables;
using System;
using Dungeon.MapSystem;

namespace Dungeon.UI
{
    public class ArchitectUITab : BaseUITab
    {
        public int material = 0;
        public bool ground = true;
        private RectInt? bounds;
        public List<IExtendedTile> selectedTiles;
        public List<Vector3Int> selectedCoords;
        public bool building = false;

        void Start()
        {
            selectedTiles = new List<IExtendedTile>();
            Statics.SelectionBox.OnSelectionFinished += SelectionFinished;
            material = (int)UIManager.UIModes.None;
            gameObject.transform.Find("GroundSwitch").gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonGroundSwitchCallBack());
        }

        public override void SetInactive()
        {
            base.SetInactive();
            building = false;
        }

        protected override void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace)
        {
            
        }

        protected override void OnItemSelected(Button button, BaseUIElementHolder item)
        {
            base.OnItemSelected(button, item);
            if (item.Data != int.MinValue)
            {
                building = true;
                material = item.Data;
            }
            //if (objectGhost != null) Destroy(objectGhost);
            // objectGhost = Instantiate(currentElement.SpawnObject);
            //ghostRenderer = objectGhost.GetComponent<SpriteRenderer>();
            //ghostRenderer.sortingLayerName = "IGUI";
        }

        private void ButtonGroundSwitchCallBack()
        {
            ground = !ground;
            if (ground)
            {
                gameObject.transform.Find("GroundSwitch").Find("Text").GetComponent<Text>().text = "Foreground";
            }
            else
            {
                gameObject.transform.Find("GroundSwitch").Find("Text").GetComponent<Text>().text = "Background";
            }
        }

        private void SelectionFinished(RectInt selection)
        {
            if (building && !MouseInputUIBlocker.BlockedByUI)
            {
                if (selection.size.x > 0 && selection.size.y > 0) bounds = selection;
                else
                {
                    var coords = Statics.TileMapFG.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    selectedCoords.Add(coords);
                    selectedTiles.Add(Statics.TileMapFG.GetTile<ExtendedRuleTile>(coords));
                }
                ExecuteOperations();
                ClearSelection();
            }
        }

        public void Build(int index)
        {
            foreach (Vector3Int tile in selectedCoords)
            {
                Statics.MapManager.ReplaceTile(ground, tile, newData: Statics.TileDictionary[Enum.GetName(typeof(Register.TileTypes), index)]);
            }
        }

        public void Mine()
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                Statics.MapManager.BreakTile(ground, selectedCoords[i]);
            }
        }

        public void ExecuteOperations()
        {
            if (building)
            {
                if (bounds.HasValue)
                    GetAllTiles(bounds.Value);
                if (material == -1)
                {
                    Mine();
                }
                else
                {
                    Build(material);
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
                    selectedTiles.Add(Statics.TileMapFG.GetTile<ExtendedRuleTile>(new Vector3Int(i, j, 0)));
                    selectedCoords.Add(new Vector3Int(i, j, 0));
                }
            }
        }
    }
}

