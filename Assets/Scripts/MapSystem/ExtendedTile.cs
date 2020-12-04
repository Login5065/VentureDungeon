using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public class ExtendedTile : Tile, IExtendedTile
    {
        public TileBase AsTile => this;
        public ExtendedTileData TileData { get; set; }
        public ExtendedTileData? TileWhenBroken { get; private set; }

        public void PreTileChanged(IExtendedTile currentTile)
        {
        }
    }
}
