namespace Dungeon.Creatures
{
    public class AttackModule : CreatureModule
    {
        public int animator;
        public float range;
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
