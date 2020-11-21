using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeon.Objects
{
    public static class ObjectList
    {
        public static Dictionary<Vector2Int, PlaceableObject> placedObjects = new Dictionary<Vector2Int, PlaceableObject>();

        public static IEnumerable<Treasure> GetTreasures()
            => placedObjects.Values.OfType<Treasure>();

        public static int GetTotalTreasureGoldValue()
            => GetTreasures().Sum(x => x.currentGold);
    }
}