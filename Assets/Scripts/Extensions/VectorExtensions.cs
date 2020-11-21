using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dungeon.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2Int Down(this Vector2Int vector) => new Vector2Int(vector.x, vector.y - 1);
        public static Vector2Int Top(this Vector2Int vector) => new Vector2Int(vector.x, vector.y + 1);
        public static Vector2Int Left(this Vector2Int vector) => new Vector2Int(vector.x - 1, vector.y);
        public static Vector2Int Right(this Vector2Int vector) => new Vector2Int(vector.x + 1, vector.y);
        public static Vector3Int ToVec3(this Vector2Int vector) => (Vector3Int)vector;
    }
}
