using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.MapSystem;
using Dungeon.Variables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.Json
{
    class TilemapConverter : JsonConverter<(Tilemap, string)[]>
    {
        private readonly (Tilemap, string)[] current;

        public TilemapConverter()
        { }

        public TilemapConverter(params Tilemap[] current)
            => this.current = current.Select(x => (x, x.name)).ToArray();

        public TilemapConverter(params (Tilemap, string)[] current)
            => this.current = current;

        public override (Tilemap, string)[] ReadJson(JsonReader reader, Type objectType, (Tilemap, string)[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (current == null)
                throw new NullReferenceException($"Deserialization needs the {nameof(current)} parameter provided!");
            else if (current.Length <= 0)
                return Array.Empty<(Tilemap, string)>();

            var jObject = serializer.Deserialize<JObject>(reader);

            // Get the default bounds
            var bounds = jObject["bounds"].ToObject<JObject>();
            var xMin = bounds["xMin"].ToObject<int>();
            var yMin = bounds["yMin"].ToObject<int>();
            var xSize = bounds["xSize"].ToObject<int>();
            var ySize = bounds["ySize"].ToObject<int>();
            var defaultBounds = new BoundsInt(xMin, yMin, 0, xSize, ySize, 0);
            // Get the dictionary of our string ids which are mapped to int values
            var tilesIdDict = jObject["tileIdDict"].ToObject<Dictionary<int, string>>();

            foreach (var (value, name) in current)
            {
                var data = jObject[name].ToObject<JObject>();
                var currentBounds = defaultBounds;

                // If the tilemap that we're reading has non-default bounds, use them instead
                if (data.ContainsKey("bounds"))
                {
                    var tempBounds = data["bounds"].ToObject<JObject>();
                    xMin = tempBounds["xMin"].ToObject<int>();
                    yMin = tempBounds["yMin"].ToObject<int>();
                    xSize = tempBounds["xSize"].ToObject<int>();
                    ySize = tempBounds["ySize"].ToObject<int>();
                    currentBounds = new BoundsInt(xMin, yMin, 0, xSize, ySize, 0);
                }

                // Read our tiles
                var tileIds = data["tileIds"]
                    .ToArray()
                    .Select(x => x.ToObject<int>())
                    .ToArray();

                int index = 0;
                value.ClearAllTiles();
                //value.SetTilesBlock(); did not work for some reason, rewrote it a bit
                for (int y = currentBounds.yMin; y < currentBounds.yMax; y++)
                {
                    for (int x = currentBounds.xMin; x < currentBounds.xMax; x++)
                    {
                        var key = tileIds[index++];

                        // Get an actual (string) id recognized by the game from our int mapped ids
                        var trueId = tilesIdDict[key];

                        if (trueId == null) continue;
                        // We'll need a way to handle tiles that use a subclass of ExtendedTile, but good enough for now
                        var tile = ScriptableObject.CreateInstance<ExtendedRuleTile>();
                        tile.TileData = Statics.TileDictionary[trueId];
                        tile.m_DefaultColliderType = tile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                        value.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
                value.CompressBounds();
            }

            return current;
        }

        public override void WriteJson(JsonWriter writer, (Tilemap, string)[] values, JsonSerializer serializer)
        {
            if ((values?.Length ?? 0) <= 0)
                return;

            writer.WriteStartObject();

            var tilesCasted = Enumerable.Empty<ExtendedRuleTile>();

            // Get every single tile from each tilemap
            foreach (var (value, _) in values)
                tilesCasted = value.GetTilesBlock(value.cellBounds).Cast<ExtendedRuleTile>().Select(x => x == null || string.IsNullOrWhiteSpace(x.TileData.TileId) ? null : x).Concat(tilesCasted);

            // Get each distinct ExtendedTileData
            var tilesIndexed = tilesCasted
                .Select(x => x == null ? default(ExtendedTileData?) : x.TileData)
                .Distinct()
                .Select((data, index) => (data, index));

            // Map an int for every single possible ExtendedTileData
            var tilesIdDict = tilesIndexed.ToDictionary(x => x.index, x => Statics.TileDictionary.TileData.FirstOrDefault(dict => x.data?.TileId == dict.TileId).TileId);

            BoundsInt? defaultBounds = null;

            foreach (var (value, name) in values)
            {
                value.CompressBounds();
                var bounds = value.cellBounds;
                var tiles = value.GetTilesBlock(bounds);

                // Assume first entry if no default bounds
                if (defaultBounds == null)
                {
                    defaultBounds = value.cellBounds;
                    writer.WritePropertyName("bounds");
                    serializer.Serialize(writer, new { defaultBounds.Value.xMin, defaultBounds.Value.yMin, xSize = defaultBounds.Value.size.x, ySize = defaultBounds.Value.size.y });
                    writer.WritePropertyName("tileIdDict");
                    serializer.Serialize(writer, tilesIdDict);
                }

                writer.WritePropertyName(name);
                writer.WriteStartObject();

                // If the bounds of the tilemap we're reading are different that the default one - save them as well
                if (value.cellBounds != defaultBounds)
                {
                    writer.WritePropertyName("bounds");
                    serializer.Serialize(writer, new { value.cellBounds.xMin, value.cellBounds.yMin, xSize = value.cellBounds.size.x, ySize = value.cellBounds.size.y });
                }

                var tilesIds = tiles
                    .Cast<ExtendedRuleTile>()
                    // Map each of our tiles as an int to save space
                    .Select(x => tilesIdDict.FirstOrDefault(dict => dict.Value == x.GetDictionaryKey()).Key)
                    .ToArray();

                writer.WritePropertyName("tileIds");
                serializer.Serialize(writer, tilesIds);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}