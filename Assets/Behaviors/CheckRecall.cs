using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using Dungeon.Variables;
using System.Linq;

public class CheckRecall : Conditional
{
    public Creature creature;
    bool recall = false;

    public override void OnAwake()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        recall = (ObjectManager.GetTreasures().Count() == 0 || creature.health / creature.maxHealth < 0.34f) && creature.recallPosition != null;
        if (recall) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}