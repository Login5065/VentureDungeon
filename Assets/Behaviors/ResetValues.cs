using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;
using UnityEngine;

public class ResetValues : Action
{
    private Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
        creature.anchor = (Vector2Int)Statics.TileMapFG.WorldToCell(creature.Position);
    }

    public override TaskStatus OnUpdate()
    {
        creature.moving = false;
        if (creature.currentMoveAction != null)
        {
            creature.previousMoveAction = creature.currentMoveAction;
            creature.currentMoveAction = null;
        }
        if (creature.previousMoveAction != null && !(creature.previousMoveAction is IAnchorless)) 
            creature.anchor = (Vector2Int)Statics.TileMapFG.WorldToCell(creature.Position);
        return TaskStatus.Failure;
    }
}

public interface IAnchorless
{ }