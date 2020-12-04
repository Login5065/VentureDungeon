using UnityEngine;

public class creator : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("dsdss");
        ItemWorld.PlaceItemWorld(new Vector3(0, 0, 0), new Item { itemType = Item.ItemType.Gold_Idol, amount = 1 });
        ItemWorld.PlaceItemWorld(new Vector3(2, 2, 0), new Item { itemType = Item.ItemType.Health_Potion, amount = 1 });
       
    }
}
