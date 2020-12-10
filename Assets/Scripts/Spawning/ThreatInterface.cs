using System.Collections.Generic;
using UnityEngine;
using Dungeon.Creatures;
using System.Linq;

namespace Dungeon.Spawning
{
    public abstract class Upgrades
    {
        public abstract int Price { get; }
        public abstract void Upgrade(Creature creature);
    }

    public class Health : Upgrades
    {
        public override int Price => 10;

        public override void Upgrade(Creature creature)
        {
            creature.maxHealth += creature.maxHealth * 0.2f;
        }
    }

    public class Resource : Upgrades
    {
        public override int Price => 20;

        public override void Upgrade(Creature creature)
        {
            creature.maxResource += creature.maxResource * 0.2f;
        }
    }

    public class Speed : Upgrades
    {
        public override int Price => 25;

        public override void Upgrade(Creature creature)
        {
            creature.speed += creature.speed * 0.5f;
        }
    }

    public class Armor : Upgrades
    {
        public override int Price => 40;

        public override void Upgrade(Creature creature)
        {
            creature.armor += 5;
        }
    }

    public class ThreatInterface : MonoBehaviour
    {
        public int DailyThreat;
        public List<Upgrades> upgradesList;

        private void Start()
        {
            upgradesList = new List<Upgrades>
            {
                new Health(),
                new Resource(),
                new Speed(),
                new Armor()
            };
        }

        public void UpgradeCreature(Creature creature)
        {
            DailyThreat = Variables.GameData.threat;

            while (DailyThreat > 0)
            {
                var possibleUpgrades = upgradesList.Where(x => x.Price <= DailyThreat);

                if (!possibleUpgrades.Any())
                    break;

                var chosen = possibleUpgrades.ElementAt(UnityEngine.Random.Range(0, possibleUpgrades.Count()));
                DailyThreat -= chosen.Price;
                chosen.Upgrade(creature);
            }

            creature.Health = creature.maxHealth;
            creature.resource = creature.maxResource;
        }
    }

}