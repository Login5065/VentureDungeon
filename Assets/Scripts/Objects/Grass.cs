using Dungeon.Variables;
using UnityEngine;

namespace Dungeon.Objects
{
    public class Grass : PlaceableObject
    {
        public override bool CanSell => false;
        public override int GoldValue => 0;
        public override void Start()
        {
            gridPosition = (Vector2Int)Statics.TileMapFG.WorldToCell(gameObject.transform.position);
            base.Start();
        }
    }
}