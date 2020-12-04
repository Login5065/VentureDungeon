using System;
using System.Collections.Generic;
using Dungeon.MapSystem;
using Dungeon.Objects;
using Dungeon.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class ObjectsUIManager : MonoBehaviour
    {
        private readonly List<Button> ObjectsButtons = new List<Button>();
        public GameObject objectSelected;
        public GameObject objectSelectedGhost;
        public GameObject objectGhost;
        private float checkDelay = 0;
        private int currentOptionSelected = -1;
        private int currentCost = -1;

        private void Start()
        {
            foreach (var item in Enum.GetNames(typeof(Register.ObjectTypes)))
            {
                ObjectsButtons.Add(Statics.LeftMenuObjects.transform.Find("Button" + item).GetComponent<Button>());
            }

            for (int i = 0; i < ObjectsButtons.Count; i++)
            {
                var x = i;
                var button = ObjectsButtons[x];
                button.onClick.AddListener(() => PlaceObjectCallback(x));
            }
        }

        private void Update()
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.Object)
            {
                if (objectSelected != null)
                {
                    var click = Input.GetMouseButtonDown(0);
                    if ((click || checkDelay <= 0) && !UI.MouseInputUIBlocker.BlockedByUI)
                    {
                        HandlePlacement(click);
                        checkDelay = 0.05f;
                    }
                    else checkDelay -= Time.unscaledDeltaTime;
                }
                else Statics.UIManager.mode = (int)UIManager.UIModes.None;
            }
            else
            {
                currentOptionSelected = -1;
                objectSelected = null;
                if (objectGhost != null)
                {
                    Destroy(objectGhost);
                    objectGhost = null;
                }
            }
        }

        private void HandlePlacement(bool place)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var tilePos = Statics.TileMapFG.WorldToCell(mousePos);
            var tile = Statics.TileMapFG.GetTile<ExtendedTile>(tilePos);

            if (ObjectList.placedObjects.ContainsKey((Vector2Int)tilePos) || (tile != null && tile.TileData.IsSolid))
                HandleGhost(mousePos, false);
            else
            {
                if (GameData.Gold >= currentCost || currentCost <= 0)
                {
                    HandleGhost(tilePos, true);
                    if (place)
                    {
                        var created = Instantiate(objectSelected, Statics.TileMapFG.CellToWorld(tilePos) + new Vector3(0.5f, 0), Quaternion.identity).GetComponent<PlaceableObject>();
                        created.GridPosition = (Vector2Int)tilePos;
                        ObjectList.placedObjects[created.GridPosition] = created;
                        GameData.Gold -= currentCost;
                    }
                }
                else HandleGhost(tilePos, false);
            }
        }

        private void HandleGhost(Vector3Int tilePos, bool canPlace)
            => HandleGhost(Statics.TileMapFG.CellToWorld(tilePos) + new Vector3(0.5f, 0), canPlace);

        private void HandleGhost(Vector3 pos, bool canPlace)
        {
            if (objectGhost == null)
            {
                objectGhost = Instantiate(objectSelectedGhost, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            }

            objectGhost.transform.position = pos;
            if (canPlace)
                objectGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 0.5f);
            else
                objectGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 0.5f); ;
        }

        private void PlaceObjectCallback(int x)
        {
            if (x == currentOptionSelected) return;
            Destroy(objectGhost);
            objectSelected = null;
            objectSelectedGhost = null;
            objectGhost = null;
            currentCost = -1;
            objectSelected = Resources.Load<GameObject>("Objects/" + Enum.GetName(typeof(Register.ObjectTypes), x));
            objectSelectedGhost = Resources.Load<GameObject>("Objects/" + Enum.GetName(typeof(Register.ObjectTypes), x) + "Ghost");
            if (objectSelected != null)
            {
                Statics.UIManager.mode = (int)UIManager.UIModes.Object;
                var text = ObjectsButtons[x].GetComponentInChildren<Text>();
                if (text != null && int.TryParse(text.text.Substring(0, text.text.Length - 3), out var cost))
                    currentCost = cost;
            }
            else currentOptionSelected = -1;
        }
    }
}
