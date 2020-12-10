using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Pathfinding;
using Dungeon.Variables;

public class Recall : Action
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        creature.Path = TilemapPathfinder.FindPathToOrBelowInt(Statics.TileMapFG, creature.recallPosition.Value, (Vector2Int)Statics.TileMapFG.WorldToCell(creature.transform.position), Mathf.CeilToInt(creature.height));
        if (Vector2.Distance(Statics.TileMapFG.CellToWorld((Vector3Int)creature.recallPosition.Value), creature.transform.position) < 2.0f) CreatureManager.KillCreature(creature, false);
        if (creature.path.Count > 0)
        {
            creature.currentMoveAction = this;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}