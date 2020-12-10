using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class CheckRez : Conditional
{
    public Creature creature;
    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }
    public override TaskStatus OnUpdate()
    {
        if (!creature.dying) return TaskStatus.Success;
        else return TaskStatus.Running;
    }
}