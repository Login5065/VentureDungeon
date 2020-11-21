using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public class ExtendedTile : Tile
    {
        private ExtendedTileData tileData;

        public ExtendedTileData TileData
        {
            get => tileData;
            set
            {
                tileData = value;
                sprite = tileData.Sprite;
            }
        }

        /// <summary>Return tile to be used when broken. Override and in ExtendedTile subclass to allow for more dynamic tile values.</summary>
        public virtual ExtendedTileData? TileWhenBroken { get; private set; }

        public virtual void PreTileChanged(ExtendedTile currentTile)
        {

        }
    }
}