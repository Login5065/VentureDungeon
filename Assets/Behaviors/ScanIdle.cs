using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;
using Dungeon.Pathfinding;

public class ScanIdle : Action, IAnchorless
{
    public Creature creature;
    protected int timeToRecalculate = 0;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        if (creature.Path.Count > 0 && creature.previousMoveAction == this)
        {
            creature.currentMoveAction = this;
            return TaskStatus.Success;
        }
        if (timeToRecalculate-- > 0)
        {
            return TaskStatus.Failure;
        }
        else timeToRecalculate = Random.Range(2, 8);
        var startingPos = (Vector2Int)Statics.TileMapFG.WorldToCell(creature.transform.position);
        var start = Random.Range(-4, 4);
        while (start != 0)
        {
            var pos = new Vector2Int(creature.anchor.x + start, creature.anchor.y);
            var newPath = TilemapPathfinder.FindPathToInt(Statics.TileMapFG, pos, startingPos, Mathf.CeilToInt(creature.height));
            if (newPath != null && newPath.Count <= 8)
            {
                creature.Path = newPath;
                break;
            }
            if (start > 0) start--;
            else start++;
        }
        if (creature.Path.Count > 0)
        {
            creature.currentMoveAction = this;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}