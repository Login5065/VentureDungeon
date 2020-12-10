using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using Dungeon.Extensions;

public class OrderToPath : Action
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        Vector3? target = null;

        if (creature.moveOrder != null)
        {
            target = creature.moveOrder.transform.position;
            Object.Destroy(creature.moveOrder);
            creature.moveOrder = null;
        }
        else if (creature.previousMoveAction == this && creature.Path.Count > 0)
            target = creature.Path[creature.Path.Count - 1].ToVec3();

        if (target != null)
        {
            var path = TilemapPathfinder.FindPathToOrBelow(Statics.TileMapFG, target.Value, creature.Position, Mathf.CeilToInt(creature.height));

            if (path != null && path.Count > 0)
            {
                creature.Path = path;
                creature.currentMoveAction = this;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }
}