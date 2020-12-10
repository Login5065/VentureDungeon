using Dungeon.Creatures;
using UnityEngine;

namespace Dungeon.Variables
{
    public static class CreatureManager
    {
        public static IntObjectRegister<Creature> register = new IntObjectRegister<Creature>();
        public static int SpawnCreature(GameObject monster, int x, int y)
        {
            var spawned = Object.Instantiate(monster, Statics.TileMapFG.CellToWorld(new Vector3Int(x, y, 0)) + new Vector3(0.5f, 0), Quaternion.identity).GetComponent<Creature>();
            int id = register.AddObject(spawned);
            spawned.enabled = true;
            spawned.GetComponent<SpriteRenderer>().sortingOrder = 1000 + id;
            spawned.transform.Find("HP_UI").GetComponent<SpriteRenderer>().sortingOrder = 3000 + id;
            spawned.transform.Find("HP_UI").Find("HP").GetComponent<SpriteRenderer>().sortingOrder = 2000 + id;
            return id;
        }

        public static bool KillCreature(Creature creature, bool giveRewards = true)
        {
            if (creature == null) return false;
            if (giveRewards)
            {
                if (creature.value != 0) GameData.Gold += creature.value;
                if (!creature.allegiance) GameData.Fame += creature.creatureCost/100;
                if (creature.allegiance)
                {
                    int ob = (int)Random.Range(0.0f, 10.0f);
                    switch (ob)
                    {
                        default:
                            break;
                        case 1:
                            ItemWorld.PlaceItemWorld(new Vector3(creature.lastPosition.x, creature.lastPosition.y + 3, creature.lastPosition.z), new Item { itemType = Item.ItemType.Gold_Idol, amount = 1 });
                            break;
                        case 2:
                            ItemWorld.PlaceItemWorld(new Vector3(creature.lastPosition.x, creature.lastPosition.y + 3, creature.lastPosition.z), new Item { itemType = Item.ItemType.Health_Potion, amount = 1 });
                            break;
                        case 3:
                            ItemWorld.PlaceItemWorld(new Vector3(creature.lastPosition.x, creature.lastPosition.y + 3, creature.lastPosition.z), new Item { itemType = Item.ItemType.Mana_Potion, amount = 1 });
                            break;
                        case 4:
                            ItemWorld.PlaceItemWorld(new Vector3(creature.lastPosition.x, creature.lastPosition.y + 3, creature.lastPosition.z), new Item { itemType = Item.ItemType.Shield, amount = 1 });
                            break;
                        case 5:
                            ItemWorld.PlaceItemWorld(new Vector3(creature.lastPosition.x, creature.lastPosition.y + 3, creature.lastPosition.z), new Item { itemType = Item.ItemType.Sword, amount = 1 });
                            break;
                    }
                    GameData.Fame -= 1;
                    GameData.Threat += 10;
                }
            }
            
            creature.dying = true;
            creature.health = 0;
            creature.ChangeAnimationState("Die");
            creature.shouldBeSeen = false;
            creature.hasSight = false;
            creature.outerhpmaterial.AddOperation(-0.1f, "_FadeAmount", 0.11f, 1.0f);
            creature.hpmaterial.AddOperation(-0.1f, "_FadeAmount", 0.11f, 1.0f);
            return true;
        }

        public static bool ResurrectCreature(Creature creature)
        {
            if (creature == null) return false;
            creature.dying = false;
            creature.health = creature.maxHealth;
            creature.ChangeAnimationState("Idle");
            creature.shouldBeSeen = true;
            creature.hasSight = true;
            creature.outerhpmaterial.AddOperation(1.0f, "_FadeAmount", 0.11f, -0.1f);
            creature.hpmaterial.AddOperation(1.0f, "_FadeAmount", 0.11f, -0.1f);
            return true;
        }
        public static bool CleanCreature(Creature creature)
        {
            register.RemoveObject(creature);
            Object.Destroy(creature.gameObject);
            return true;
        }
    }
}

