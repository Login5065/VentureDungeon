using UnityEngine;

namespace Dungeon.Extensions
{
    public static class FloatExtensions
    {
        public static bool Approximately(this float value, float other)
            => Mathf.Approximately(value, other);
    }
}
