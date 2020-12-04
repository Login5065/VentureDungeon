using System;
using System.Diagnostics;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon.MapSystem
{
    [JsonObject]
    [Serializable]
    [DebuggerDisplay("Tile: {TileId}")]
    public struct ExtendedTileData : IEquatable<ExtendedTileData>
    {
        // For unity editor
        [SerializeReference]
        private TileBase tileToUse;
        [SerializeField]
        private string tileId;
        [SerializeField]
        private bool isSolid;
        [SerializeField]
        private int workToBreak;
        [SerializeField]
        private bool canClimb;
        [SerializeField]
        private bool isPlatform;
        [SerializeField]
        private int tileMatchingLayer;

        // Exposed to stuff that's not unity editor
        [JsonIgnore]
        public IExtendedTile TileToUse { get => tileToUse as IExtendedTile; set => tileToUse = value.AsTile; }
        public string TileId { get => tileId; private set => tileId = value; }
        public bool IsSolid { get => isSolid; private set => isSolid = value; }
        [JsonIgnore]
        public bool CanBreak => WorkToBreak >= 0;
        public int WorkToBreak { get => workToBreak; private set => workToBreak = value; }
        public bool CanClimb { get => canClimb; private set => canClimb = value; }
        public bool IsPlatform { get => isPlatform; private set => isPlatform = value; }
        public int TileMatchingLayer { get => tileMatchingLayer; private set => tileMatchingLayer = value; }
        [JsonIgnore]
        public bool CanStandOn => IsSolid || CanClimb || IsPlatform;

        [JsonConstructor]
        public ExtendedTileData(string tileId, bool isSolid = false, int workToBreak = -1, bool canClimb = false, bool isPlatform = false, int tileMatchingLayer = 0)
        {
            tileToUse = null;
            this.tileId = tileId;
            this.isSolid = isSolid;
            this.workToBreak = workToBreak;
            this.canClimb = canClimb;
            this.isPlatform = isPlatform;
            this.tileMatchingLayer = tileMatchingLayer;
        }

        public override bool Equals(object obj) => obj is ExtendedTileData data && Equals(data);

        public bool Equals(ExtendedTileData other) => other.TileId == TileId;

        public override int GetHashCode() => TileId.GetHashCode();

        public static bool operator ==(ExtendedTileData left, ExtendedTileData right) => left.Equals(right);

        public static bool operator !=(ExtendedTileData left, ExtendedTileData right) => !(left == right);
    }
}