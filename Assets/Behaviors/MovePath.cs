using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class MovePath : Action
{
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        if (creature.Path.Count > 0)
        {
            creature.moving = true;
            creature.ChangeAnimationState("Walk");
            return TaskStatus.Success;
        }
        else
        {
            creature.ChangeAnimationState("Idle");
            return TaskStatus.Failure;
        }
    }
}