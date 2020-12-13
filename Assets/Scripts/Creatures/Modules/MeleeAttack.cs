using System.Collections.Generic;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class MeleeAttack : AttackModule
    {
        public float attack = 10;
        public bool pierce = false;
        private HashSet<Creature> toRemove;
        public void Start()
        {
            toRemove = new HashSet<Creature>();
        }
        public override bool Requirement()
        {
            return (owner.closestCreature != null && Vector2.Distance(owner.transform.position, owner.closestCreature.transform.position) < range);
        }
        public override bool Attack()
        {
            owner.isAttacking = true;
            owner.ChangeAnimationState(attackAnimation);
            return true;
        }
        public override bool ExecuteBonk()
        {
            owner.audioSource.PlayOneShot(soundEffect);
            void EndBonk()
            {
                owner.seenCreatures.Remove(owner.closestCreature);
                owner.isAttacking = false;
                if (owner.Path.Count > 0)
                    owner.Path.Clear();
            }
            if (owner.closestCreature == null)
            {
                EndBonk();
                return false;
            }
            if(pierce)
            {
                toRemove.Clear();
                bool allDead = true;
                foreach (Creature c in owner.seenCreatures)
                {
                    if(Vector2.Distance(owner.transform.position, c.transform.position) < range)
                    {
                        c.Health -= attack - c.armor;
                        if (c.Health <= 0)
                        {
                            toRemove.Add(owner.closestCreature);
                        }
                        else
                        {
                            allDead = false;
                        }
                    }
                }
                foreach (Creature c in toRemove)
                {
                    owner.seenCreatures.Remove(c);
                }
                if (allDead)
                {
                    EndBonk();
                }
            }
            else
            {
                owner.closestCreature.Health -= attack - owner.closestCreature.armor;
                if (owner.closestCreature.Health <= 0)
                {
                    EndBonk();
                }
            }
            return true;
        }
    }
}