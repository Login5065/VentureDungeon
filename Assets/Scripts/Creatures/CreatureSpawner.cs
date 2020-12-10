using System.Collections;
using System.Linq;
using Dungeon.Variables;
using UnityEngine;
using Dungeon.Spawning;

namespace Dungeon.Creatures
{
    public class CreatureSpawner : MonoBehaviour
    {
        public Vector2Int spawnDespawnPoint;

        public static float mainSpawnCooldown;
        public static bool done = false;

        public void StartSpawning() => StartCoroutine(SpawnCreatures());

        private IEnumerator SpawnCreatures()
        {
            mainSpawnCooldown = 30f;
            var spawnCount = FameInterface.spawnList.Count;
            done = false;
            while(true)
            {
                if (ObjectManager.GetTreasures().Count() > 0)
                {
                    yield return new WaitForSeconds(5.0f);
                    for (int i = 0; i < spawnCount; i++)
                    {
                        var id = CreatureManager.SpawnCreature(FameInterface.spawnList[i].gameObject, spawnDespawnPoint.x, spawnDespawnPoint.y);
                        CreatureManager.register[id].recallPosition = spawnDespawnPoint;
                        yield return new WaitForSeconds(Random.Range(3.0f, 30.0f));
                    }
                    done = true;
                    yield break;
                }
                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}
