using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class CheckBusy : Conditional
{
    private Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (creature.busy) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}