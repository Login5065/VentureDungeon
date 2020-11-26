namespace Dungeon.Creatures
{
    public class AnimateIdle : IdleModule
    {
        public override bool Requirement()
        {
            return true;
        }
        public override bool Idle()
        {
            owner.ChangeAnimationState("Idle");
            return true;
        }
    }
}