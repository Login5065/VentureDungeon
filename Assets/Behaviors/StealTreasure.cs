using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Objects;
using Dungeon.Creatures;
using Dungeon.Variables;
using System.Linq;

public class StealTreasure : Action
{
    Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        Treasure closeTreasure = ObjectManager.GetTreasures().FirstOrDefault(x => x.currentGold > 0 && CanPickUp(x));
        if (closeTreasure != null)
        {
            creature.ChangeAnimationState("Idle");
            var stolen = Mathf.Min(closeTreasure.currentGold, 10);
            closeTreasure.currentGold -= stolen;
            GameData.Fame += stolen / 10;
            if (closeTreasure.currentGold <= 0) ObjectManager.KillObject(closeTreasure);
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }

    private bool CanPickUp(Treasure x) => Mathf.Abs(x.transform.position.x - creature.transform.position.x) <= (creature.width / 2) + 0.1f && Mathf.Abs(x.transform.position.y - (creature.height / 2) - creature.transform.position.y) <= (creature.height / 2) + 0.1f;
}