using Dungeon.MapSystem;

namespace Dungeon.Extensions
{
    public static class ExtendedTileExtensions
    {
        public static string GetDictionaryKey(this ExtendedTile tile)
        {
            if (tile == null || tile.TileData.Sprite == null)
                return null;
            else
                return tile.TileData.Sprite.name;
        }
    }
}
