using System.Collections;
using Dungeon.Creatures;
using System.Collections.Generic;
using UnityEngine;
using Dungeon.Variables;
using System.Linq;
using Dungeon.Spawning;

public class DayEvent : EventTemplate
{
    public int Requirment = 5;
    public int addition = 5;
    public int importance = 1;
    public int time=0;
    public override bool Check()
    {
        Debug.Log("curr Day"+ Statics.DayNightManager.DaysPassed);
        if (Statics.DayNightManager.DaysPassed >= Requirment)
        {
            Requirment += addition;
            return true;
        }
        return false;
    }

    public override void Disable()
    {
        throw new System.NotImplementedException();
    }

    public override void Enable()
    {
        Debug.Log("Summoning");
        time++;
        Creature creature = heroesList[R];
        creature.maxHealth += creature.maxHealth * 2 * time; ;
        creature.maxResource += creature.maxResource *3 * time;
        creature.speed += creature.speed /2;
        creature.armor += 10*time;
        creature.creatureCost = (int)(creature.creatureCost * time * 2);
        CreatureManager.SpawnCreature(creature.gameObject, Statics.creatureSpawner.spawnDespawnPoint.x, Statics.creatureSpawner.spawnDespawnPoint.y);
    }

    public override int getImportance()
    {
        return importance;
    }

}
