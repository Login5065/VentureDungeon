using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class AnimateIdle : Action
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        creature.ChangeAnimationState("Idle");
        return TaskStatus.Success;
    }
}