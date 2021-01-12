using Dungeon.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTemplate 
{
    public static List<Creature> heroesList = new List<Creature>()
        {
            Resources.Load<GameObject>("Creatures/SwordHero").GetComponent<Creature>(),
            Resources.Load<GameObject>("Creatures/SpearHero").GetComponent<Creature>(),
            Resources.Load<GameObject>("Creatures/BowHero").GetComponent<Creature>()
        };
    public abstract bool Check();
    public abstract void Enable();
    public abstract void Disable();
    public abstract int getImportance();
    public int R => Random.Range(0, (heroesList.Count - 1));
}
