using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class CheckSeekTreasure : Conditional
{
    public Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (creature.seeksTreasure) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}