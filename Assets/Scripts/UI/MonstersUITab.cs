using System.Collections.Generic;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using Dungeon.Creatures;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class MonstersUITab : BaseUITab
    {
        protected GameObject monsterGhost;
        protected SpriteRenderer ghostRenderer;

        protected override void OnItemSelected(Button button, BaseUIElementHolder item)
        {
            base.OnItemSelected(button, item);
            CursorManager.SetCursor("Attack");
            if (monsterGhost != null) Destroy(monsterGhost); 
            monsterGhost = Instantiate(currentElement.SpawnObject);
            monsterGhost.transform.Find("HP_UI").gameObject.SetActive(false);
            ghostRenderer = monsterGhost.GetComponent<SpriteRenderer>();
            ghostRenderer.sortingLayerName = "IGUI";
        }

        protected override void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace)
        {
            var tilePos = Statics.TileMapFG.WorldToCell(mousePos);

            TilemapPathfinder.GetClosesValidPointBelow(Statics.TileMapFG, ref tilePos);
            if (TilemapPathfinder.CanStandOn(Statics.TileMapFG, (Vector2Int)tilePos) && TilemapPathfinder.CanFit(Statics.TileMapFG, (Vector2Int)tilePos, 3))
            {
                if (GameData.Gold >= currentElement.Cost || currentElement.Cost <= 0)
                {
                    HandleGhost(tilePos, true);
                    if (shouldTryPlace)
                    {
                        CreatureManager.SpawnCreature(currentElement.SpawnObject, tilePos.x, tilePos.y);
                        if (currentElement.Cost > 0) GameData.Gold -= currentElement.Cost;
                    }
                }
                else HandleGhost(tilePos, false);
            }
            else HandleGhost(mousePos, false);
        }

        public override void SetInactive()
        {
            base.SetInactive();
            Destroy(monsterGhost);
            monsterGhost = null;
        }

        protected void HandleGhost(Vector3Int tilePos, bool canPlace)
            => HandleGhost(Statics.TileMapFG.CellToWorld(tilePos) + new Vector3(0.5f, 0), canPlace);

        protected void HandleGhost(Vector3 pos, bool canPlace)
        {
            monsterGhost.transform.position = pos;
            if (canPlace)
                ghostRenderer.color = new Color(0, 1f, 0, 0.5f);
            else
                ghostRenderer.color = new Color(1f, 0, 0, 0.5f);
        }
    }
}
