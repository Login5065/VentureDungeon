using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;
using UnityEngine;

public class CheckWallStuck : Conditional
{
    private Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (creature.Path.Count > 0 && (Statics.TileMapFG.GetTile(Statics.TileMapFG.WorldToCell(creature.Position)) != null || Statics.TileMapFG.GetTile(Statics.TileMapFG.WorldToCell(creature.Position) + Vector3Int.down) == null)) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}