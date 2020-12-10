using System.Collections.Generic;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using Dungeon.Creatures;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Dungeon.Objects;

namespace Dungeon.UI
{
    public class ObjectsUITab : BaseUITab
    {
        protected GameObject objectGhost;
        protected SpriteRenderer ghostRenderer;
        protected bool floating;

        protected override void OnItemSelected(Button button, BaseUIElementHolder item)
        {
            base.OnItemSelected(button, item);
            if (objectGhost != null) Destroy(objectGhost);
            objectGhost = Instantiate(currentElement.SpawnObject);
            ghostRenderer = objectGhost.GetComponent<SpriteRenderer>();
            ghostRenderer.sortingLayerName = "IGUI";
            floating = currentElement.SpawnObject.GetComponent<PlaceableObject>().floating;
        }

        protected override void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace)
        {
            var tilePos = Statics.TileMapFG.WorldToCell(mousePos);

            if (floating)
            {
                if (TilemapPathfinder.CanFit(Statics.TileMapFG, (Vector2Int)tilePos, 3))
                {
                    if (GameData.Gold >= currentElement.Cost || currentElement.Cost <= 0)
                    {
                        HandleGhost(tilePos, true);
                        if (shouldTryPlace)
                        {
                            ObjectManager.SpawnObject(currentElement.SpawnObject, tilePos.x, tilePos.y);
                            if (currentElement.Cost > 0) GameData.Gold -= currentElement.Cost;
                        }
                    }
                    else HandleGhost(tilePos, false);
                }
                else HandleGhost(mousePos, false);
            }
            else
            {
                TilemapPathfinder.GetClosesValidPointBelow(Statics.TileMapFG, ref tilePos);
                if (TilemapPathfinder.CanStandOn(Statics.TileMapFG, (Vector2Int)tilePos) && TilemapPathfinder.CanFit(Statics.TileMapFG, (Vector2Int)tilePos, 3))
                {
                    if (GameData.Gold >= currentElement.Cost || currentElement.Cost <= 0)
                    {
                        HandleGhost(tilePos, true);
                        if (shouldTryPlace)
                        {
                            ObjectManager.SpawnObject(currentElement.SpawnObject, tilePos.x, tilePos.y);
                            if (currentElement.Cost > 0) GameData.Gold -= currentElement.Cost;
                        }
                    }
                    else HandleGhost(tilePos, false);
                }
                else HandleGhost(mousePos, false);
            }
        }

        public override void SetInactive()
        {
            base.SetInactive();
            Destroy(objectGhost);
            objectGhost = null;
        }

        protected void HandleGhost(Vector3Int tilePos, bool canPlace)
            => HandleGhost(Statics.TileMapFG.CellToWorld(tilePos) + new Vector3(0.5f, 0), canPlace);

        protected void HandleGhost(Vector3 pos, bool canPlace)
        {
            objectGhost.transform.position = pos;
            if (canPlace)
                ghostRenderer.color = new Color(0, 1f, 0, 0.5f);
            else
                ghostRenderer.color = new Color(1f, 0, 0, 0.5f);
        }
    }
}
