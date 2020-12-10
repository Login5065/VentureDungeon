using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;

public class Attack : Action
{
    bool hit = false;
    Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        hit = creature.chosenAttack.Attack();
        if (hit) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}