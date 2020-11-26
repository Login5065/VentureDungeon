namespace Dungeon.Creatures
{
    public class AttackModule : CreatureModule
    {
        public float range;
        public string attackAnimation = "Attack1";
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
