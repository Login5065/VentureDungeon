using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;

public class Die : Action
{
    private Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!creature.dying)
        {
            CreatureManager.KillCreature(creature);
        }
        return TaskStatus.Success;
    }
}