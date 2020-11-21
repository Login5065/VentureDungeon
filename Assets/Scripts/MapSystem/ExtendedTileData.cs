using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Dungeon.MapSystem
{
    [JsonObject]
    public struct ExtendedTileData : IEquatable<ExtendedTileData>
    {
        public Sprite Sprite { get; set; }
        
        public bool IsSolid { get; private set; }
        [JsonIgnore]
        public bool CanBreak => WorkToBreak >= 0;
        public int WorkToBreak { get; private set; }
        public bool CanClimb { get; private set; }
        public bool IsPlatform { get; private set; }
        [JsonIgnore]
        public bool CanStandOn => IsSolid || CanClimb || IsPlatform;

        [JsonConstructor]
        public ExtendedTileData(Sprite sprite, bool isSolid = false, int workToBreak = -1, bool canClimb = false, bool isPlatform = false)
        {
            Sprite = sprite;
            IsSolid = isSolid;
            WorkToBreak = workToBreak;
            CanClimb = canClimb;
            IsPlatform = isPlatform;
        }

        public override bool Equals(object obj)
        {
            return obj is ExtendedTileData data && Equals(data);
        }

        public bool Equals(ExtendedTileData other)
        {
            return Sprite.name == other.Sprite.name &&
                   IsSolid == other.IsSolid &&
                   WorkToBreak == other.WorkToBreak &&
                   CanClimb == other.CanClimb &&
                   IsPlatform == other.IsPlatform;
        }

        public override int GetHashCode()
        {
            int hashCode = 296606536;
            hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(Sprite);
            hashCode = hashCode * -1521134295 + IsSolid.GetHashCode();
            hashCode = hashCode * -1521134295 + WorkToBreak.GetHashCode();
            hashCode = hashCode * -1521134295 + CanClimb.GetHashCode();
            hashCode = hashCode * -1521134295 + IsPlatform.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ExtendedTileData left, ExtendedTileData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExtendedTileData left, ExtendedTileData right)
        {
            return !(left == right);
        }
    }
}