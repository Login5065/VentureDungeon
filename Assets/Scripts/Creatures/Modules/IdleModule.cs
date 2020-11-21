namespace Dungeon.Creatures
{
    public class IdleModule : CreatureModule
    {
        public virtual bool Idle()
        {
            return false;
        }
    }
}