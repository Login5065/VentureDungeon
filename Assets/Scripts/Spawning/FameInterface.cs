using System.Collections.Generic;
using System.Linq;
using Dungeon.Creatures;
using Dungeon.Variables;
using UnityEngine;

namespace Dungeon.Spawning
{
    public static class FameInterface
    {
        public static List<Creature> heroesList = new List<Creature>()
        {
            Resources.Load<GameObject>("Creatures/SwordHero").GetComponent<Creature>(),
            Resources.Load<GameObject>("Creatures/SpearHero").GetComponent<Creature>(),
            Resources.Load<GameObject>("Creatures/BowHero").GetComponent<Creature>()
        };
        public static List<Creature> spawnList = new List<Creature>();

        public static void ListHeroesToSpawn()
        {
            var DailyFame = GameData.fame;
            spawnList.Clear();

            while (DailyFame > 0)
            {
                var possibleHeroes = heroesList.Where(x => x.GoldValue <= DailyFame);

                if (possibleHeroes.Count() == 0)
                    break;

                var chosen = possibleHeroes.ElementAt(Random.Range(0, possibleHeroes.Count()));
                DailyFame -= chosen.GoldValue;
                spawnList.Add(chosen);
            }
        }
    }
}