using Dungeon.Objects;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using System.Linq;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class RecallIdle : IdleModule
    {
        public override bool Requirement()
        {
            return ((ObjectList.GetTreasures().Count() == 0 || (owner.carriedGoldTarget > 0 && owner.carriedGold >= owner.carriedGoldTarget)) && owner.spawnerObject != null );
        }
        public override bool Idle()
        {
            owner.idleBacktrackPath.Clear();
            owner.Path = TilemapPathfinder.FindPathToOrBelowInt(Statics.TileMapFG, owner.spawnerObject.spawnDespawnPoint, (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position), Mathf.CeilToInt(owner.height));
            if (Vector2.Distance(Statics.TileMapFG.CellToWorld((Vector3Int)owner.spawnerObject.spawnDespawnPoint), owner.transform.position) < 2.0f) Destroy(owner.gameObject);
            if (owner.Path.Count > 0)
            {
                Vector3 distcalc = Statics.TileMapFG.CellToWorld(new Vector3Int(owner.Path[0].x, owner.Path[0].y, 0));
                distcalc.x += 0.5f;
                if (Vector2.Distance(owner.transform.position, distcalc) < 0.1)
                {
                    owner.Path.RemoveAt(0);
                }
                if (owner.Path.Count != 0)
                {
                    owner.ChangeAnimationState("Walk");
                    Vector2 movePosition = Statics.TileMapFG.CellToWorld(new Vector3Int(owner.Path[0].x, owner.Path[0].y, 0));
                    movePosition.x += 0.5f;
                    owner.transform.position = Vector2.MoveTowards(owner.transform.position, movePosition, owner.speed * Time.deltaTime);
                }
            }
            return true;
        }
    }
}