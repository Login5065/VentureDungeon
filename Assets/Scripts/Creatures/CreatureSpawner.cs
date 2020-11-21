using System.Collections;
using System.Linq;
using Dungeon.Extensions;
using Dungeon.Variables;
using Dungeon.Objects;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class CreatureSpawner : MonoBehaviour
    {
        public Vector2Int spawnDespawnPoint;
        public GameObject creatureToSpawn;

        public static float mainSpawnCooldown;
        public static int spawnCount;
        public static float cooldownBetweenSpawns;
        public static bool firstSpawn = false;

        private void Start()
            => StartCoroutine(SpawnCreatures());

        private IEnumerator SpawnCreatures()
        {
            mainSpawnCooldown = 30f;
            cooldownBetweenSpawns = 1f;
            spawnCount = 1;
            while(true)
            {
                if (Variables.GameData.gameStarted && ObjectList.GetTreasures().Count() > 0)
                {
                    if (firstSpawn)
                    {
                        yield return new WaitForSeconds(5.0f);
                        firstSpawn = false;
                    }
                    for (int i = 0; i < spawnCount; i++)
                    {
                        var creature = Instantiate(creatureToSpawn, Statics.TileMapFG.CellToWorld(spawnDespawnPoint.ToVec3()), Quaternion.identity).GetComponent<Creature>();
                        creature.enabled = true;
                        creature.timeToRecalculatePathToTreasure = 0f;
                        creature.spawnerObject = this;
                        yield return new WaitForSeconds(cooldownBetweenSpawns);
                    }
                    yield return new WaitForSeconds(mainSpawnCooldown);
                }
                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}
