using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;

public class Undie : Action
{
    private Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        CreatureManager.ResurrectCreature(creature);
        return TaskStatus.Success;
    }
}