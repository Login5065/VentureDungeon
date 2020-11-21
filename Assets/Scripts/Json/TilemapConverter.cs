using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.MapSystem;
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

                // Read our tiles and their rotation (if any)
                var tileIds = data["tileIds"]
                    .ToArray()
                    .Select(x => x.ToObject<JObject>())
                    .Select(x => (key: x["k"].ToObject<int>(), rotation: x["r"].ToObject<byte>()))
                    .ToArray();

                int index = 0;
                value.ClearAllTiles();
                //value.SetTilesBlock(); did not work for some reason, rewrote it a bit
                for (int y = currentBounds.yMin; y < currentBounds.yMax; y++)
                {
                    for (int x = currentBounds.xMin; x < currentBounds.xMax; x++)
                    {
                        var (key, rotation) = tileIds[index++];

                        // Get an actual (string) id recognized by the game from our int mapped ids
                        var trueId = tilesIdDict[key];

                        if (trueId == null) continue;
                        // We'll need a way to handle tiles that use a subclass of ExtendedTile, but good enough for now
                        var tile = ScriptableObject.CreateInstance<ExtendedTile>();
                        tile.TileData = TileDictionary.TileData[trueId];
                        tile.colliderType = tile.TileData.IsSolid ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                        int rotationZ = 0;
                        int rotationY = 0;

                        // Read our rotation values
                        if ((rotation & 0b100) != 0)
                            rotationY = 180;
                        switch (rotation & 0b011)
                        {
                            case 0b01:
                                rotationZ = 90;
                                break;
                            case 0b10:
                                rotationZ = 180;
                                break;
                            case 0b11:
                                rotationZ = 270;
                                break;
                        }

                        // Set the rotation and tile
                        tile.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, rotationY, rotationZ), Vector3.one);
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

            var tilesCasted = Enumerable.Empty<ExtendedTile>();

            // Get every single tile from each tilemap
            foreach (var (value, _) in values)
                tilesCasted = value.GetTilesBlock(value.cellBounds).Cast<ExtendedTile>().Select(x => x == null || x.TileData.Sprite == null ? null : x).Concat(tilesCasted);

            // Get each distinct ExtendedTileData
            var tilesIndexed = tilesCasted
                .Select(x => x == null ? default(ExtendedTileData?) : x.TileData)
                .Distinct()
                .Select((data, index) => (data, index));

            // Map an int for every single possible ExtendedTileData
            var tilesIdDict = tilesIndexed.ToDictionary(x => x.index, x => TileDictionary.TileData.FirstOrDefault(dict => x.data == dict.Value).Key);

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
                    .Cast<ExtendedTile>()
                    .Select(x =>
                    {
                        // Map each of our tiles as an int to save space
                        var key = tilesIdDict.FirstOrDefault(dict => dict.Value == x.GetDictionaryKey()).Key;

                        // Since our rotation has limited possible states that make sense for 2D game with square tiles, we save it as a masked byte
                        // Skipping value of 0 would save more, but would require changing the way it's serialized, so let's not bother for now
                        byte rotation = 0b000;
                        if (x != null)
                        {
                            var angles = x.transform.rotation.eulerAngles;

                            if (angles.z.Approximately(90f))
                                rotation = 0b001;
                            else if (angles.z.Approximately(180f))
                                rotation = 0b010;
                            else if (angles.z.Approximately(270f))
                                rotation = 0b011;

                            if (angles.y.Approximately(180f))
                                rotation |= 0b100;
                        }
                        
                        return new { k = key, r = rotation };
                    })
                    .ToArray();

                writer.WritePropertyName("tileIds");
                serializer.Serialize(writer, tilesIds);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}