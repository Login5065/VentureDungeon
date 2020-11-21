using UnityEngine;

namespace Dungeon.Creatures
{
    public class MeleeAttack : AttackModule
    {
        public float attack = 10;
        public override bool Requirement()
        {
            return (owner.closestCreature != null && Vector2.Distance(owner.transform.position, owner.closestCreature.transform.position) < range);
        }

        public override bool Attack()
        {
            owner.isAttacking = true;
            owner.animator.SetInteger("Anim", 2);
            return true;
        }
        public override bool ExecuteBonk()
        {
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
            owner.closestCreature.Health -= attack - owner.closestCreature.armor;
            if (owner.closestCreature.Health <= 0)
            {
                EndBonk();
                return true;
            }
            return true;
        }
    }
}