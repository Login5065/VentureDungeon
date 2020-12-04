using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dungeon.Json;
using Newtonsoft.Json;
using UnityEngine;

namespace Dungeon.MapSystem
{
    public class TileDictionary : MonoBehaviour
    {
        [SerializeField]
        private List<ExtendedTileData> tileData;
        public List<ExtendedTileData> TileData { get => tileData; private set => tileData = value; }
        public ExtendedTileData this[string id]
        {
            get => TileData.Where(x => x.TileId == id).FirstOrDefault();
            set
            {
                TileData.RemoveAll(x => x.TileId == value.TileId);
                TileData.Add(value);
            }
        }

        public bool ContainsKey(string key) => TileData.Any(x => x.TileId == key);
    }
}