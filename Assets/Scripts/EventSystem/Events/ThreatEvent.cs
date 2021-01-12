using Dungeon.Variables;
using System.Collections;
using Dungeon.Creatures;
using System.Collections.Generic;
using UnityEngine;

public class ThreatEvent : EventTemplate
{
    public int Requirment = 100;
    public int addition = 100;
    public int importance = 3;
    public int time = 0;
    public override bool Check()
    {
        if (Dungeon.Variables.GameData.threat >= Requirment)
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
        time++;

        for (int i = 0; i < time; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Creature creature = heroesList[R];
                int random = Random.Range(0, 3);
                switch (random)
                {
                    case 0:
                        creature.maxResource += creature.maxResource * 3 * time;
                        break;
                    case 1:
                        creature.speed += creature.speed / 2;
                        break;
                    case 2:
                        creature.armor += 10 * time;
                        break;

                };
                creature.maxHealth += creature.maxHealth * time; ;
                creature.creatureCost = (int)(creature.creatureCost  * time* 1.4);
                CreatureManager.SpawnCreature(creature.gameObject, Statics.creatureSpawner.spawnDespawnPoint.x, Statics.creatureSpawner.spawnDespawnPoint.y);
            }
        }




       
    }

    public override int getImportance()
    {
        return importance;
    }

  
}
