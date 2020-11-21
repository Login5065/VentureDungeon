using System;
using System.IO;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.Json;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public class MapManager : MonoBehaviour
    {
        public Tilemap tilemap;
        public Tilemap bgtilemap;
        public ExtendedTile this[int x, int y, bool ground = true] => this[new Vector3Int(x, y, 0), ground];
        public ExtendedTile this[Vector2Int pos, bool ground = true] => this[pos.ToVec3(), ground];
        public ExtendedTile this[Vector3Int pos, bool ground = true]
        {
            get => ground switch
            {
                true => tilemap.GetTile<ExtendedTile>(pos),
                false => bgtilemap.GetTile<ExtendedTile>(pos),
            };
            set => ReplaceTile(ground, pos, value);
        }

        public bool loadDefaultMapFileOnStartup = true;

        // Start is called before the first frame update
        void Start()
        {
            TileDictionary.LoadTileData("base_tiles.json");
            if (loadDefaultMapFileOnStartup)
                LoadMap("test_map.json");
        }

        public ExtendedTile BreakTile(bool ground, Vector3Int pos, Type typeToCreate = null)
            => BreakTile(ground, pos, Matrix4x4.identity, typeToCreate);

        public ExtendedTile BreakTile(bool ground, Vector3Int pos, Matrix4x4 transform, Type typeToCreate = null)
        {
            ExtendedTile tile;
            if (ground)
            {
                tile = tilemap.GetTile<ExtendedTile>(pos);
            }
            else
            {
                tile = bgtilemap.GetTile<ExtendedTile>(pos);
            }
            ExtendedTileData? newTileData = null;
            if (tile != null)
            {
                newTileData = tile.TileWhenBroken;
            }

            return ReplaceTile(ground, pos, transform, currentTile: tile, newData: newTileData, typeToCreate: typeToCreate);
        }

        public ExtendedTile ReplaceTile(bool ground, Vector3Int pos, ExtendedTile currentTile = null, ExtendedTileData? newData = null, ExtendedTile newTile = null, Type typeToCreate = null)
            => ReplaceTile(ground, pos, Matrix4x4.identity, currentTile, newData, newTile, typeToCreate);

        public ExtendedTile ReplaceTile(bool ground, Vector3Int pos, Matrix4x4 transform, ExtendedTile currentTile = null, ExtendedTileData? newData = null, ExtendedTile newTile = null, Type typeToCreate = null)
        {
            if (newTile == null && newData != null && typeToCreate?.IsAssignableFrom(typeof(ExtendedTile)) != false)
            {
                newTile = ScriptableObject.CreateInstance(typeToCreate ?? typeof(ExtendedTile)) as ExtendedTile;
                newTile.TileData = newData.Value;
            }

            if (currentTile == null)
                if (ground)
                {
                    currentTile = tilemap.GetTile<ExtendedTile>(pos);
                }
                else
                {
                    currentTile = bgtilemap.GetTile<ExtendedTile>(pos);
                }
            if (newTile != null)
            {
                newTile.transform = transform;
                newTile.PreTileChanged(currentTile);
            }
            if (ground)
            {
                tilemap.SetTile(pos, newTile);
            }
            else
            {
                bgtilemap.SetTile(pos, newTile);
            }

            return newTile;
        }

        public void SaveMap()
        {
            var tilemaps = new[] { tilemap, bgtilemap };
            var newEntries = 0;

            foreach (var map in tilemaps)
            {
                var sprites = map.GetTilesBlock(map.cellBounds).OfType<Tile>().Select(x => x.sprite).Distinct();

                foreach (var sprite in sprites)
                {
                    if (!TileDictionary.TileData.ContainsKey(sprite.name))
                    {
                        newEntries++;
                        TileDictionary.TileData[sprite.name] = new ExtendedTileData(sprite, true, 100);
                    }
                }
            }

            if (newEntries > 0)
                TileDictionary.SaveFileData("new_data.json");

            foreach (var map in tilemaps)
            {
                for (int y = map.cellBounds.yMin; y < map.cellBounds.yMax; y++)
                {
                    for (int x = map.cellBounds.xMin; x < map.cellBounds.xMax; x++)
                    {
                        var pos = new Vector3Int(x, y, 0);

                        if (map.GetTile(pos) is Tile currentTile)
                        {
                            var newTile = ScriptableObject.CreateInstance<ExtendedTile>();
                            newTile.TileData = TileDictionary.TileData[currentTile.sprite.name];
                            newTile.transform = map.GetTransformMatrix(pos);
                            newTile.colliderType = newTile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                            map.SetTransformMatrix(pos, Matrix4x4.identity);
                            map.SetTile(pos, newTile);
                        }
                    }
                }
            }

            File.WriteAllText("new_test_map.json", JsonConvert.SerializeObject(new[] { (tilemap, tilemap.name), (bgtilemap, bgtilemap.name) }, new TilemapConverter()));
        }

        public void LoadMap(string path)
        {
            if (File.Exists(path))
                JsonConvert.DeserializeObject<(Tilemap, string)[]>(File.ReadAllText(path), new TilemapConverter(tilemap, bgtilemap));
        }
    }
}
