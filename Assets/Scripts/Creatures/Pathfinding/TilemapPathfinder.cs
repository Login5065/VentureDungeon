using System.Collections.Generic;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.MapSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Dungeon.Pathfinding
{
    public static class TilemapPathfinder
    {
        private const int defaultPrice = 15;

        public static List<Vector2Int> FindPathToOrBelow(Tilemap tilemap, in Vector3 targetPos, in Vector3 startingPos, in int creatureSize, int maxSearchDistance = int.MaxValue) 
            => FindPathToOrBelowInt(tilemap, (Vector2Int)tilemap.WorldToCell(targetPos), (Vector2Int)tilemap.WorldToCell(startingPos), creatureSize, maxSearchDistance);

        public static List<Vector2Int> FindPathToOrBelowInt(Tilemap tilemap, Vector2Int targetPos, Vector2Int startingPos, in int creatureSize, int maxSearchDistance = int.MaxValue)
        {
            GetClosesValidPointBelow(tilemap, ref targetPos, maxSearchDistance);
            return FindPathToInt(tilemap, targetPos, startingPos, creatureSize);
        }

        public static List<Vector2Int> FindPathTo(Tilemap tilemap, in Vector3 targetPos, in Vector3 startingPos, in int creatureSize)
            => FindPathToInt(tilemap, (Vector2Int)tilemap.WorldToCell(targetPos), (Vector2Int)tilemap.WorldToCell(startingPos), creatureSize);

        public static List<Vector2Int> FindPathToInt(Tilemap tilemap, in Vector2Int targetPos, in Vector2Int startingPos, in int creatureSize)
        {
            Assert.IsTrue(creatureSize > 0, "Creature size must be bigger than 0");

            var startNode = new PathfindingNode(startingPos);

            var openSet = new Dictionary<Vector2Int, PathfindingNode>();
            var closedSet = new Dictionary<Vector2Int, PathfindingNode>();

            if (!tilemap.cellBounds.Contains((Vector3Int)startingPos))
                return null;

            if(targetPos == startingPos)
                return new List<Vector2Int>() { targetPos };

            if (!tilemap.cellBounds.Contains((Vector3Int)targetPos))
                return null;

            openSet.Add(startingPos, startNode);

            while (openSet.Count > 0)
            {
                // Find a node with lowest cost
                var node = openSet.OrderBy(x => x.Value).First().Value;

                // Remove the current node from open set and add it to closed one, as it has the lowest cost from all possible nodes
                openSet.Remove(node.Pos);
                closedSet.Add(node.Pos, node);

                // If we've found our target - retrace the path
                if (node == targetPos)
                    return RetracePath(startNode, node);

                foreach (var neighbourNode in GetValidNeighbours(tilemap, node, creatureSize, closedSet))
                {
                    if (openSet.TryGetValue(neighbourNode.Pos, out var existing))
                    {
                        // The node exists in the open set, we check if the current path has lower cost and if yes we update the existing node's path and cost
                        if (neighbourNode.GeneratedCost < existing.GeneratedCost)
                        {
                            existing.Parent = node;
                            existing.GeneratedCost = neighbourNode.GeneratedCost;
                        }
                    }
                    // The node with specific pos does not exist in open set, we add it and calculate the approximate cost
                    else
                    {
                        neighbourNode.Parent = node;
                        neighbourNode.HeuristicCost = Distance(neighbourNode, targetPos);
                        openSet.Add(neighbourNode.Pos, neighbourNode);
                    }
                }
            }

            return null;
        }

        private static IEnumerable<PathfindingNode> GetValidNeighbours(Tilemap tilemap, PathfindingNode currentNode, int creatureSize, Dictionary<Vector2Int, PathfindingNode> closedSet)
        {
            // TODO: Use some actually meaningful values for movement cost
            // Most likely make it more expensive to climb up/down, etc.
            var pos = currentNode.Pos.Top();
            if (!closedSet.ContainsKey(pos) && tilemap.cellBounds.Contains(pos.ToVec3()) && CanStandOn(tilemap, pos) && CanFit(tilemap, pos, creatureSize))
                yield return new PathfindingNode(pos, currentNode.GeneratedCost + defaultPrice);

            pos = currentNode.Pos.Right();
            if (!closedSet.ContainsKey(pos) && tilemap.cellBounds.Contains(pos.ToVec3()) && CanStandOn(tilemap, pos) && CanFit(tilemap, pos, creatureSize))
                yield return new PathfindingNode(pos, currentNode.GeneratedCost + defaultPrice);

            pos = currentNode.Pos.Left();
            if (!closedSet.ContainsKey(pos) && tilemap.cellBounds.Contains(pos.ToVec3()) && CanStandOn(tilemap, pos) && CanFit(tilemap, pos, creatureSize))
                yield return new PathfindingNode(pos, currentNode.GeneratedCost + defaultPrice);

            // TODO: Consider if this needs CanFit, since technically we are in a spot that we fit in alread, and only move 1 tile down
            // We could only check that one tile then, but doesn't hurt to leave it like that for now
            pos = currentNode.Pos.Down();
            if (!closedSet.ContainsKey(pos) && tilemap.cellBounds.Contains(pos.ToVec3()) && CanStandOn(tilemap, pos) && CanFit(tilemap, pos, creatureSize))
                yield return new PathfindingNode(pos, currentNode.GeneratedCost + defaultPrice);
        }

        private static List<Vector2Int> RetracePath(PathfindingNode start, PathfindingNode end)
        {
            var path = new List<Vector2Int>();

            for (var currentNode = end; currentNode != start; currentNode = currentNode.Parent)
                path.Add(currentNode);

            path.Reverse();
            return path;
        }

        // Manhattan distance - generally used when only allowed to move in 4 directions (so no diagonal movements)
        // If we allowed diagonal movements, instead of adding those values we'd pick the higher one - octile/Chebyshev distance
        // For free movement we'd use Euclidean distance
        private static int Distance(in Vector2Int current, in Vector2Int target)
            => Mathf.Abs(current.x - target.x) + Mathf.Abs(current.y - target.y) * defaultPrice;

        public static bool CanFit(Tilemap tilemap, in Vector2Int posToCheck, int creatureSize)
        {
            var currentPos = posToCheck;
            for (var i = 0; i < creatureSize; i++)
            {
                var tile = tilemap.GetTile<ExtendedTile>(currentPos.ToVec3());

                if (tile != null && tile.TileData.IsSolid)
                    return false;

                currentPos = currentPos.Top();
            }

            return true;
        }

        public static bool CanStandOn(Tilemap tilemap, in Vector2Int posToCheck)
        {
            var tile = tilemap.GetTile<ExtendedTile>(posToCheck.Down().ToVec3());
            return tile != null && tile.TileData.CanStandOn;
        }

        public static void GetClosesValidPointBelow(Tilemap tilemap, ref Vector2Int targetPos, int maxSearchDistance = int.MaxValue)
        {
            while (maxSearchDistance-- > 0 && !CanStandOn(tilemap, targetPos) && (tilemap.cellBounds.yMax <= targetPos.y || tilemap.cellBounds.Contains(targetPos.ToVec3())))
                targetPos.y -= 1;
        }

        public static void GetClosesValidPointBelow(Tilemap tilemap, ref Vector3Int targetPos, int maxSearchDistance = int.MaxValue)
        {
            while (maxSearchDistance-- > 0 && !CanStandOn(tilemap, (Vector2Int)targetPos) && (tilemap.cellBounds.yMax <= targetPos.y || tilemap.cellBounds.Contains(targetPos)))
                targetPos.y -= 1;
        }
    }
}
