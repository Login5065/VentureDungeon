using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public class ExtendedRuleTile : RuleTile<RuleTile.TilingRule>, IExtendedTile
    {
        public TileBase AsTile => this;

        public ExtendedTileData TileData { get; set; }

        /// <summary>Return tile to be used when broken. Override and in ExtendedTile subclass to allow for more dynamic tile values.</summary>
        public virtual ExtendedTileData? TileWhenBroken { get; private set; }

        public virtual void PreTileChanged(IExtendedTile currentTile)
        {
        }

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is ExtendedRuleTile tile && tile.TileData.TileMatchingLayer >= 0)
            {
                switch (neighbor)
                {
                    case TilingRule.Neighbor.This: return tile.TileData.TileMatchingLayer == TileData.TileMatchingLayer;
                    case TilingRule.Neighbor.NotThis: return tile.TileData.TileMatchingLayer != TileData.TileMatchingLayer;
                }
            }
            return base.RuleMatch(neighbor, other);
        }
    }
}