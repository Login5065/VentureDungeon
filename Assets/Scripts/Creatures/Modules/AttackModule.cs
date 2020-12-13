using UnityEngine;

namespace Dungeon.Creatures
{
    public class AttackModule : CreatureModule
    {
        public float range;
        public string attackAnimation = "Attack1";
        public AudioClip soundEffect;
        public virtual bool Attack()
        {
            return false;
        }
        public virtual bool ExecuteBonk()
        {
            return false;
        }
    }
}
