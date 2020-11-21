using System;
using UnityEngine;

namespace Dungeon.Pathfinding
{
    internal class PathfindingNode : IComparable<PathfindingNode>, IEquatable<PathfindingNode>
    {
        public PathfindingNode Parent { get; set; }
        public Vector2Int Pos { get; set; }
        // Cost to move from the start position to here
        public int GeneratedCost { get; set; }
        // Estimated cost to move from here to end position
        public int HeuristicCost { get; set; }
        public int FullCost => GeneratedCost + HeuristicCost;
        
        public PathfindingNode(Vector2Int pos, int gCost = 0, int hCost = 0, PathfindingNode parent = null)
        {
            Pos = pos;
            GeneratedCost = gCost;
            HeuristicCost = hCost;
            Parent = parent;
        }

        public int CompareTo(PathfindingNode other) => FullCost - other.FullCost;

        public override bool Equals(object obj) => obj is PathfindingNode node && Equals(node);

        public bool Equals(PathfindingNode other) => Pos.Equals(other.Pos);

        public override int GetHashCode() => 1731973265 + Pos.GetHashCode();

        public static bool operator ==(PathfindingNode left, PathfindingNode right) => left.Equals(right);

        public static bool operator !=(PathfindingNode left, PathfindingNode right) => !(left == right);

        public static implicit operator Vector2Int(PathfindingNode node) => node.Pos;

        public override string ToString() => $"({Pos.x}, {Pos.y})";
    }
}
