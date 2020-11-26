using Dungeon.Pathfinding;
using Dungeon.Variables;
using System.Linq;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class WanderIdle : IdleModule
    {
        public override bool Requirement()
        {
            return (!owner.isAttacking && owner.Path.Count == 0);
        }
        public override bool Idle()
        {
            if (owner.idleBacktrackPath.Count == 0) owner.idleBacktrackPath.Add((Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position));

            // Too far from last idle pos, go back
            var lastPos = owner.idleBacktrackPath.Last();
            if (Vector2Int.Distance(lastPos, (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position)) >= 2)
            {
                owner.Path = TilemapPathfinder.FindPathToInt(Statics.TileMapFG, lastPos, (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position), Mathf.CeilToInt(owner.height));

                // Can't find path to last pos, try the starting pos instead
                if (owner.Path.Count == 0 && owner.idleBacktrackPath.Count > 1)
                {
                    owner.Path = TilemapPathfinder.FindPathToInt(Statics.TileMapFG, owner.idleBacktrackPath.First(), (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position), Mathf.CeilToInt(owner.height));
                    owner.idleBacktrackPath.RemoveRange(1, owner.idleBacktrackPath.Count - 2);
                }

                // We could not find path to the last idle positions, set this as the new ones
                if (owner.Path.Count == 0)
                {
                    owner.idleBacktrackPath.Clear();
                    owner.idleBacktrackPath.Add((Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position));
                }
            }

            if (owner.idleTimer <= 0)
            {
                owner.idleTimer = Random.Range(5f, 20f);

                // We're too far from the starting pos, backtrack
                if (owner.idleBacktrackPath.Count > 5)
                {
                    while (owner.idleBacktrackPath.Count > 5)
                    {
                        owner.idleBacktrackPath.RemoveAt(owner.idleBacktrackPath.Count - 1);
                        owner.Path.Add(owner.idleBacktrackPath.Last());
                    }
                }
                else
                {
                    // Random sign to not have clockwise/counterclockwise preference for checking directions
                    var sign = Random.value < 0.5f ? -1 : 1;
                    var value = Random.Range(0, 4);

                    var startingPos = (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position);

                    for (int checks = 0; checks < 4; checks++)
                    {

                        // Pick one of the 4 possible directions to check directions
                        var pickedDirection = value switch
                        {
                            0 => Vector2Int.up,
                            1 => Vector2Int.right,
                            2 => Vector2Int.down,
                            _ => Vector2Int.left,
                        };
                        var pos = startingPos;
                        var maxSteps = Random.Range(1, 4);

                        for (int step = 1; step <= maxSteps; step++)
                        {
                            // We move by one step in the picked direction
                            pos += pickedDirection;

                            // Try to get the new path to the target
                            var newPath = TilemapPathfinder.FindPathToInt(Statics.TileMapFG, pos, (Vector2Int)Statics.TileMapFG.WorldToCell(owner.transform.position), Mathf.CeilToInt(owner.height));
                            // Make sure we take the most direct path
                            if (newPath?.Count == step)
                                owner.Path = newPath;
                            // If the current path is not valid we end early
                            else
                                break;

                            // If we're backtracking - remove the last step from the backtrack path
                            if (owner.idleBacktrackPath.Count > 1 && pos == owner.idleBacktrackPath[owner.idleBacktrackPath.Count - 2])
                                owner.idleBacktrackPath.RemoveAt(owner.idleBacktrackPath.Count - 1);
                            // If we're not backtracking, add the new step to backtrack path (unless it's the starting pos)
                            else if (pos != owner.idleBacktrackPath[0])
                                owner.idleBacktrackPath.Add(pos);
                        }

                        // If we have some path selected we're ending the loop
                        if (owner.Path.Count > 0) break;

                        // Increment/decrement the value and handle wrapping
                        value += sign;
                        if (value >= 4) value -= 4;
                        else if (value < 0) value += 4;
                    }
                }
                return true;
            }
            else
            {
                owner.idleTimer -= Time.deltaTime;
                return false;
            }
        }
    }
}