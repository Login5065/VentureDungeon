using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Dungeon.Pathfinding;
using Dungeon.Variables;

namespace Dungeon.Creatures
{
    public class SeesCreaturesIdle : IdleModule
    {
        public override bool Requirement()
        {
            return (owner.seenCreatures.Count != 0);
        }
        public override bool Idle()
        {
            if (owner.timeToRecalculatePathToEnemy <= 0)
                owner.timeToRecalculatePathToEnemy += 0.25f;
            else
            {
                owner.timeToRecalculatePathToEnemy -= Time.deltaTime;
                return owner.isAttacking;
            }

            float shortestDistance = float.PositiveInfinity;
            bool dirty = false;
            bool setNewPath = false;

            var ordered = owner.seenCreatures.Select(creature =>
            {
                var distance = float.PositiveInfinity;

                if (creature == null || creature.health <= 0)
                    dirty = true;
                else
                    distance = Vector3.Distance(creature.transform.position, owner.transform.position);
                return (creature, distance);
            }).OrderBy(x => x.distance);

            foreach (var (creature, distance) in ordered)
            {
                shortestDistance = distance;
                if (float.IsInfinity(distance))
                    break;

                var newPath = TryGetPathTowardsCreature(creature);

                if (newPath != null && newPath.Count <= 10)
                {
                    setNewPath = true;
                    owner.closestCreature = creature;
                    owner.Path = newPath;
                    break;
                }
            }
            if (dirty)
                owner.seenCreatures.RemoveWhere(x => x == null || x.health <= 0);
            if ((owner.isAttacking && !setNewPath) || float.IsInfinity(shortestDistance))
            {
                owner.isAttacking = false;
                return false;
            }
            owner.AttackCreature();
            return owner.isAttacking;
        }
        public List<Vector2Int> TryGetPathTowardsCreature(Creature creature)
            => TilemapPathfinder.FindPathToOrBelow(Statics.TileMapFG, creature.transform.position, owner.transform.position, Mathf.CeilToInt(owner.height));
    }
}