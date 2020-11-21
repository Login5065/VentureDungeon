using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Dungeon.Json
{
    class SpriteConverter : JsonConverter<Sprite>
    {
        public override Sprite ReadJson(JsonReader reader, Type objectType, Sprite existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (!string.IsNullOrWhiteSpace(value))
            {
                var sprites = Resources.LoadAll<Sprite>("spritesheet_tiles");
                return sprites.FirstOrDefault(s => s.name == value);
            }
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, Sprite value, JsonSerializer serializer)
            => writer.WriteValue(value != null ? value.name : string.Empty);
    }
}
