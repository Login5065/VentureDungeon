using System.Collections.Generic;
using System.IO;
using Dungeon.Json;
using Newtonsoft.Json;

namespace Dungeon.MapSystem
{
    public static class TileDictionary
    {
        public static Dictionary<string, ExtendedTileData> TileData { get; private set; } = new Dictionary<string, ExtendedTileData>();

        public static void LoadTileData(string path, bool clear = false, bool overwriteExisting = true)
        {
            if (File.Exists(path) && new FileInfo(path).Extension.ToLower() == ".json")
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, ExtendedTileData>>(File.ReadAllText(path), new SpriteConverter());

                if (clear)
                    TileData = data;
                else
                {
                    foreach (var item in data)
                    {
                        if (overwriteExisting || !TileData.ContainsKey(item.Key))
                            TileData[item.Key] = item.Value;
                    }
                }
            }
            else throw new FileNotFoundException("Could not load tile dictionary file", path);
        }

        public static void SaveFileData(string path, bool overwrite = false)
        {
            if (overwrite && File.Exists(path))
                File.Delete(path);

            if (!File.Exists(path))
                File.WriteAllText(path, JsonConvert.SerializeObject(TileData, new SpriteConverter()));
        }
    }
}