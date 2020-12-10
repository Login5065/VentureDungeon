using System.Collections.Generic;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.Objects;
using Dungeon.Pathfinding;
using UnityEngine;

namespace Dungeon.Variables
{
    public static class ObjectManager
    {
        public static ObjectRegister<Vector2Int, PlaceableObject> register = new ObjectRegister<Vector2Int, PlaceableObject>();

        public static IEnumerable<Treasure> GetTreasures() => register.objects.Values.OfType<Treasure>().Where(x => TilemapPathfinder.FindPathToOrBelowInt(Statics.TileMapFG, Statics.TileMapFG.WorldToCell(x.transform.position).ToVec2(), Statics.creatureSpawner.spawnDespawnPoint, 2) != null);
        public static IEnumerable<Entry> GetEntries() => register.objects.Values.OfType<Entry>();

        public static int GetTotalTreasureGoldValue() => GetTreasures().Sum(x => x.currentGold);

        public static void SpawnObject(GameObject obj, int x, int y) => SpawnObject(obj, new Vector2Int(x, y));

        public static void SpawnObject(GameObject obj, Vector2Int pos)
        {
            var spawned = Object.Instantiate(obj, Statics.TileMapFG.CellToWorld(pos.ToVec3()) + new Vector3(0.5f, 0), Quaternion.identity).GetComponent<PlaceableObject>();
            register.AddObject(pos, spawned);
            spawned.enabled = true;
            spawned.GridPosition = pos;
            spawned.GetComponent<SpriteRenderer>().sortingOrder = 5000;
        }

        public static void KillObject(Vector2Int pos)
        {
            if (register.TryGetValue(pos, out var value))
            {
                KillObject(value);
            }
        }

        public static void KillObject(PlaceableObject value)
        {
            if (register.Remove(value.GridPosition)) Object.Destroy(value.gameObject);
        }
    }
}
