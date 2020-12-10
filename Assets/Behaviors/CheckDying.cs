using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class CheckDying : Conditional
{
    private Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (creature.health <= 0) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}