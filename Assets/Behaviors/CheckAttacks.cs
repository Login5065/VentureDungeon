using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Dungeon.Creatures;
using System.Collections.Generic;
using System.Linq;

public class CheckAttacks : Action
{
    Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    private void RefreshList(IEnumerable<CreatureModule> objects)
    {
        foreach (CreatureModule c in objects) c.tried = false;
    }

    private IEnumerable<CreatureModule> PreparePriotities(int priority, IEnumerable<CreatureModule> objects) => objects.Where(x => x.priority == priority && !x.tried && x.Requirement());

    private CreatureModule Roll(IEnumerable<CreatureModule> objects)
    {
        if (objects.Count() == 1) return objects.First();
        float sum = objects.Sum(c => c.chance);
        double diceRoll = Random.Range(0, sum);
        sum = 0.0f;
        foreach (CreatureModule c in objects)
        {
            sum += c.chance;
            if (diceRoll < sum)
            {
                return c;
            }
        }
        return null;
    }

    public override TaskStatus OnUpdate()
    {
        bool attackFound = false;
        int attackPriority = creature.maxAttackPriority;
        RefreshList(creature.attackModules.Cast<CreatureModule>());
        IEnumerable<CreatureModule> preparedList = PreparePriotities(attackPriority, creature.attackModules.Cast<CreatureModule>());
        while (!attackFound)
        {
            preparedList = preparedList.Where(c => !c.tried);
            if (attackPriority == 0) return TaskStatus.Failure;
            if (preparedList.Count() == 0)
            {
                attackPriority -= 1;
                preparedList = PreparePriotities(attackPriority, creature.attackModules.Cast<CreatureModule>());
            }
            else
            {
                creature.chosenAttack = Roll(preparedList) as AttackModule;
                attackFound = creature.chosenAttack.Requirement();
                creature.chosenAttack.tried = true;
            }
        }
        if (attackFound) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}