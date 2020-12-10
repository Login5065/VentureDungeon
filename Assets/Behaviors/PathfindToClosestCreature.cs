using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Pathfinding;
using Dungeon.Variables;

public class PathfindToClosestCreature : Action, IAnchorless
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        creature.Path = TilemapPathfinder.FindPathToOrBelow(Statics.TileMapFG, creature.closestCreature.transform.position, creature.transform.position, Mathf.CeilToInt(creature.height));
        if (creature.Path.Count > 0)
        {
            creature.currentMoveAction = this;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}