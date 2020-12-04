using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { 
    Sword,
    Shield,
    Gold_Idol,
    Health_Potion,
    Mana_Potion,
    
    }
    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        return itemType switch
        {
            ItemType.Shield => ItemAssets.Instance.shieldSprite,
            ItemType.Gold_Idol => ItemAssets.Instance.GoldIdolSprite,
            ItemType.Health_Potion => ItemAssets.Instance.HealthPotionSprite,
            ItemType.Mana_Potion => ItemAssets.Instance.ManaPotionSprite,
            _ => ItemAssets.Instance.swordSprite,
        };
    }
}
