using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class CheckIdle : Conditional
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (creature.canIdle) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}