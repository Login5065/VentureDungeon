using System;
using System.IO;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.Json;
using Dungeon.Variables;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    public class MapManager : MonoBehaviour
    {
        public Tilemap tilemap;
        public Tilemap bgtilemap;
        public IExtendedTile this[int x, int y, bool ground = true] => this[new Vector3Int(x, y, 0), ground];
        public IExtendedTile this[Vector2Int pos, bool ground = true] => this[pos.ToVec3(), ground];
        public IExtendedTile this[Vector3Int pos, bool ground = true]
        {
            get => ground switch
            {
                true => tilemap.GetTile(pos) as IExtendedTile,
                false => bgtilemap.GetTile(pos) as IExtendedTile,
            };
            set => ReplaceTile(ground, pos, value);
        }

        public bool loadDefaultMapFileOnStartup = true;

        // Start is called before the first frame update
        void Start()
        {
            if (loadDefaultMapFileOnStartup)
                LoadMap("test_map.json");
            else
                InitTilemaps();
        }

        public IExtendedTile BreakTile(bool ground, Vector3Int pos)
        {
            IExtendedTile tile;
            if (ground)
            {
                tile = tilemap.GetTile(pos) as IExtendedTile;
            }
            else
            {
                tile = bgtilemap.GetTile(pos) as IExtendedTile;
            }
            ExtendedTileData? newTileData = null;
            if (tile != null)
            {
                newTileData = tile.TileWhenBroken;
            }

            return ReplaceTile(ground, pos, currentTile: tile, newData: newTileData);
        }

        public IExtendedTile ReplaceTile(bool ground, Vector3Int pos, IExtendedTile currentTile = null, ExtendedTileData? newData = null, IExtendedTile newTile = null)
        {
            if (newTile == null && newData?.TileToUse != null)
            {
                newTile = Instantiate(newData.Value.TileToUse.AsTile) as IExtendedTile;
            }

            if (currentTile == null)
                if (ground)
                {
                    currentTile = tilemap.GetTile(pos) as IExtendedTile;
                }
                else
                {
                    currentTile = bgtilemap.GetTile(pos) as IExtendedTile;
                }
            if (newTile != null)
            {
                newTile.PreTileChanged(currentTile);
            }
            if (ground)
            {
                tilemap.SetTile(pos, newTile?.AsTile);
            }
            else
            {
                bgtilemap.SetTile(pos, newTile?.AsTile);
            }

            return newTile;
        }

        public void InitTilemaps()
        {
            var tilemaps = new[] { tilemap, bgtilemap };

            foreach (var map in tilemaps)
            {
                for (int y = map.cellBounds.yMin; y < map.cellBounds.yMax; y++)
                {
                    for (int x = map.cellBounds.xMin; x < map.cellBounds.xMax; x++)
                    {
                        var pos = new Vector3Int(x, y, 0);
                        var tile = map.GetTile(pos);

                        if (tile is IExtendedTile extendedTile)
                        {
                            if (extendedTile.TileData.TileToUse == null || string.IsNullOrWhiteSpace(extendedTile.TileData.TileId))
                                extendedTile.TileData = Statics.TileDictionary[tile.name];
                            if (tile is Tile currentTile)
                                currentTile.colliderType = extendedTile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                            else if (tile is RuleTile currentRuleTile)
                                currentRuleTile.m_DefaultColliderType = extendedTile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                        }
                        else if (tile is Tile currentTile)
                        {
                            var newTile = ScriptableObject.CreateInstance<ExtendedTile>();
                            if (!(currentTile is IExtendedTile extended) || extended.TileData != null)
                                newTile.TileData = Statics.TileDictionary[currentTile.sprite.name];
                            newTile.colliderType = newTile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                            map.SetTransformMatrix(pos, currentTile.transform);
                            map.SetTile(pos, newTile);
                        }
                        else if (tile is RuleTile currentRuleTile)
                        {
                            var newTile = ScriptableObject.CreateInstance<ExtendedRuleTile>();
                            if (!(currentRuleTile is IExtendedTile extended) || extended.TileData == null)
                                newTile.TileData = Statics.TileDictionary[currentRuleTile.m_DefaultSprite.name];
                            newTile.m_DefaultColliderType = newTile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                            map.SetTile(pos, newTile);
                        }
                    }
                }
            }
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
                    if (!Statics.TileDictionary.ContainsKey(sprite.name))
                    {
                        newEntries++;
                        Statics.TileDictionary[sprite.name] = new ExtendedTileData(sprite.name, true, 100);
                    }
                }
            }

            InitTilemaps();

            File.WriteAllText("new_test_map.json", JsonConvert.SerializeObject(new[] { (tilemap, tilemap.name), (bgtilemap, bgtilemap.name) }, new TilemapConverter()));
        }

        public void LoadMap(string path)
        {
            if (File.Exists(path))
                JsonConvert.DeserializeObject<(Tilemap, string)[]>(File.ReadAllText(path), new TilemapConverter(tilemap, bgtilemap));
        }
    }
}
