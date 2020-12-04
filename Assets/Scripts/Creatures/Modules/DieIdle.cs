using Dungeon.Variables;
using System.Collections;
using UnityEngine;
using Dungeon.Graphics;

namespace Dungeon.Creatures
{
    public class DieIdle : IdleModule
    {
        public override bool Requirement()
        {
            return owner.health <= 0;
        }
        public override bool Idle()
        {
            owner.dying = true;
            owner.health = 0;
            owner.ChangeAnimationState("Die");
            owner.shouldBeSeen = false;
            owner.StartCoroutine(Die());
            return true;
        }

        IEnumerator Die()
        {
            if (owner.value != 0) GameData.Gold += owner.value;
            if (owner.type != 0) GameData.Fame += 1;
            if (owner.type == 0)
            {
                int ob = (int) Random.Range(0.0f, 10.0f);
                switch (ob)
                {
                    default:
                        break;
                    case 1:
                        ItemWorld.PlaceItemWorld(new Vector3(owner.lastPosition.x, owner.lastPosition.y + 3, owner.lastPosition.z), new Item { itemType = Item.ItemType.Gold_Idol, amount = 1 });
                        break;
                    case 2:
                        ItemWorld.PlaceItemWorld(new Vector3(owner.lastPosition.x, owner.lastPosition.y + 3, owner.lastPosition.z), new Item { itemType = Item.ItemType.Health_Potion, amount = 1 });
                        break;
                    case 3:
                        ItemWorld.PlaceItemWorld(new Vector3(owner.lastPosition.x, owner.lastPosition.y + 3, owner.lastPosition.z), new Item { itemType = Item.ItemType.Mana_Potion, amount = 1 });
                        break;
                    case 4:
                        ItemWorld.PlaceItemWorld(new Vector3(owner.lastPosition.x, owner.lastPosition.y + 3, owner.lastPosition.z), new Item { itemType = Item.ItemType.Shield, amount = 1 });
                        break;
                    case 5:
                        ItemWorld.PlaceItemWorld(new Vector3(owner.lastPosition.x, owner.lastPosition.y + 3, owner.lastPosition.z), new Item { itemType = Item.ItemType.Sword, amount = 1 });
                        break;
                }
                GameData.Fame -= 1;
                GameData.Threat += 10;
            }
            var mat = owner.transform.Find("HP_UI").gameObject.AddComponent<ShaderEffects>();
            mat.AddOperation(-0.1f, "_FadeAmount", 0.11f, 1.0f);
            yield return new WaitForSeconds(10);
            Destroy(owner.gameObject);
            yield break;
        }
    }
}