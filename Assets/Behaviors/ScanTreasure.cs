using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;
using Dungeon.Pathfinding;

public class ScanTreasure : Action
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        bool hasPath = false;
        if (creature.currentMoveAction != null) creature.path.Clear();
        foreach (var treasure in ObjectManager.GetTreasures())
        {
            if (treasure.currentGold > 0)
            {
                var currentPath = TilemapPathfinder.FindPathToOrBelowInt(Statics.TileMapFG, treasure.GridPosition, (Vector2Int)Statics.TileMapFG.WorldToCell(creature.transform.position), Mathf.CeilToInt(creature.height), Mathf.CeilToInt(creature.height) - 1);

                if (currentPath != null)
                {
                    if (creature.Path.Count > currentPath.Count || creature.Path.Count == 0) creature.Path = currentPath;
                    hasPath = true;
                }
            }
        }
        if (!hasPath && creature.previousMoveAction == this) creature.Path.Clear();
        if (creature.Path.Count > 0 && hasPath)
        {
            creature.currentMoveAction = this;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}