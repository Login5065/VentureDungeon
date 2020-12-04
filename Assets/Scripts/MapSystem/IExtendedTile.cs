using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public interface IExtendedTile
    {
        TileBase AsTile { get; }

        ExtendedTileData TileData { get; set; }

        ExtendedTileData? TileWhenBroken { get; }

        void PreTileChanged(IExtendedTile currentTile);
    }
}
