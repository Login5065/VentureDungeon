using Dungeon.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon.Variables;




public class ItemWorld : MonoBehaviour
{
    private Item item;
    private bool hasEntered = false;
    public bool wasPlaced = false;

    public static ItemWorld PlaceItemWorld(Vector3 position, Item item)
    {
        GameObject gameObject = Instantiate(Resources.Load<GameObject>("Items/ItemObject_pf"), position, Quaternion.identity);
        ItemWorld itemWorld = gameObject.GetComponent<ItemWorld>();
        itemWorld.item = item;
        itemWorld.wasPlaced = true;
        return itemWorld;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.hasSight && creature.allegiance == false && !hasEntered)
        {
            if (wasPlaced == true)
            {
                switch (item.itemType)
                {
                    default:
                        break;
                    case Item.ItemType.Mana_Potion:
                        Debug.Log("mana");
                        RegMana(creature);
                        break;
                    case Item.ItemType.Health_Potion:
                        Debug.Log("hp");
                        Heal(creature);
                        break;
                    case Item.ItemType.Shield:
                        Debug.Log("armor");
                        IncArmor(creature);
                        break;
                    case Item.ItemType.Sword:
                        Debug.Log("atk");
                        IncDMG(creature);
                        break;
                }
            }
            else
            {
                hasEntered = true;
                Statics.inventoryUI.inventory.AddItem(item);
                Statics.inventoryUI.RefreshInventoryItems();
                Destroy(gameObject);
            }
        }
    }
    private void Heal(Creature creature)
    {
        creature.health += 50;
        if (creature.health > creature.maxHealth)
            creature.health = creature.maxHealth;
        Destroy(this.gameObject);
    }
    private void IncDMG(Creature creature)
    {
        creature.speed += 1;
        Destroy(this.gameObject);
    }
    private void IncArmor(Creature creature)
    {
        creature.armor += 2;
        Destroy(this.gameObject);
    }
    private void RegMana(Creature creature)
    {
        creature.resource += 50;
        if (creature.resource > creature.maxResource)
            creature.resource = creature.maxResource;
        Destroy(this.gameObject);
    }
}