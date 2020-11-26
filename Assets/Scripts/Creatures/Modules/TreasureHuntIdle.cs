using Dungeon.Objects;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using System.Linq;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class TreasureHuntIdle : IdleModule
    {
        public override bool Requirement()
        {
            return (!owner.isAttacking && !owner.controllable);
        }
        public override bool Idle()
        {
            bool CanPickUp(Treasure x) => Mathf.Abs(x.transform.position.x - owner.transform.position.x) <= (owner.width / 2) + 0.1f && Mathf.Abs(x.transform.position.y - (owner.height / 2) - owner.transform.position.y) <= (owner.height / 2) + 0.1f;

            if (owner.timeToRecalculatePathToTreasure > 0)
            {
                owner.timeToRecalculatePathToTreasure -= Time.deltaTime;
                return false;
            }

            var closeTreasure = ObjectList.GetTreasures().FirstOrDefault(x => x.currentGold > 0 && CanPickUp(x));

            if (closeTreasure != null)
            {
                owner.timeToRecalculatePathToTreasure = 1f;
                owner.idleTimer = 5f;

                var stolen = Mathf.Min(closeTreasure.currentGold, 10);
                closeTreasure.currentGold -= stolen;
                owner.carriedGold += stolen;
                GameData.Fame += stolen / 10;

                if (closeTreasure.currentGold <= 0)
                    closeTreasure.Destroy();
                return true;
            }

            owner.timeToRecalculatePathToTreasure = 5f;

            foreach (var treasure in ObjectList.GetTreasures())
            {
                if (treasure.currentGold > 0)
                {
                    var currentPath = TilemapPathfinder.FindPathToOrBelowInt(Statics.TileMapFG, treasure.GridPosition, (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position), Mathf.CeilToInt(owner.height), Mathf.CeilToInt(owner.height) - 1);

                    if (currentPath != null && (owner.Path.Count > currentPath.Count || owner.Path.Count == 0))
                    {
                        owner.Path = currentPath;
                        owner.timeToRecalculatePathToTreasure = 0.5f;
                        owner.idleBacktrackPath.Clear();
                    }
                }
            }
            return true;
        }
    }
}