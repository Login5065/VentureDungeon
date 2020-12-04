using Dungeon.MapSystem;

namespace Dungeon.Extensions
{
    public static class ExtendedTileExtensions
    {
        public static string GetDictionaryKey(this ExtendedRuleTile tile)
        {
            if (tile == null || string.IsNullOrWhiteSpace(tile.TileData.TileId))
                return null;
            else
                return tile.TileData.TileId;
        }
    }
}
