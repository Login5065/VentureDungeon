namespace Dungeon.Scripts
{
    public interface ISellable
    {
        bool CanSell { get; }
        int GoldValue { get; }
    }
}
