using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using Dungeon.Creatures;
using Dungeon.Variables;
using System.Linq;

public class ScanEnemies : Action
{
    public HashSet<Creature> seenEnemies = new HashSet<Creature>();
    public Creature creature;
    public override void OnStart()
    {
        creature = gameObject.GetComponent<Creature>();
    }

    public override TaskStatus OnUpdate()
    {
        seenEnemies.Clear();
        foreach (var c in CreatureManager.register.objects.Values)
        {
            if (c.allegiance != creature.allegiance && !c.dying && Vector3.Distance(gameObject.transform.position, c.gameObject.transform.position) <= creature.sightRange)
            {
                seenEnemies.Add(c);
            }
        }
        var ordered = seenEnemies.Select(creature =>
        {
            var distance = float.PositiveInfinity;
            distance = Vector3.Distance(creature.transform.position, gameObject.transform.position);
            return (creature, distance);
        }).OrderBy(x => x.distance);
        creature.seenCreatures = new HashSet<Creature>(seenEnemies);
        creature.closestCreature = ordered.FirstOrDefault().creature;
        if (seenEnemies.Count > 0) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}