using Dungeon.Pathfinding;
using Dungeon.Variables;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class MoveIdle : IdleModule
    {
        public override bool Requirement()
        {
            return (owner.moveOrder != null || owner.Path.Count > 0);
        }
        public override bool Idle()
        {
            if (owner.moveOrder != null)
            {
                owner.Path = TilemapPathfinder.FindPathToOrBelow(Statics.TileMapFG, owner.moveOrder.transform.position, gameObject.transform.position, Mathf.CeilToInt(owner.height));
                Destroy(owner.moveOrder);
                owner.moveOrder = null;
                owner.idleBacktrackPath.Clear();
            }
            else if (owner.Path.Count > 0)
            {
                Vector3 distcalc = Statics.TileMapFG.CellToWorld(new Vector3Int(owner.Path[0].x, owner.Path[0].y, 0));
                distcalc.x += 0.5f;
                if (Vector2.Distance(this.gameObject.transform.position, distcalc) < 0.1)
                {
                    owner.Path.RemoveAt(0);
                }
                if (owner.Path.Count != 0)
                {
                    owner.animator.SetInteger("Anim", 1);
                    Vector2 movePosition = Statics.TileMapFG.CellToWorld(new Vector3Int(owner.Path[0].x, owner.Path[0].y, 0));
                    movePosition.x += 0.5f;
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, movePosition, owner.speed * Time.deltaTime);
                }
            }
            return true;
        }
    }
}