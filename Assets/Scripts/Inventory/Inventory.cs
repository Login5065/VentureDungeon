using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;
    
    public Inventory() {
       itemList = new List<Item>();
    }

    public void AddItem(Item item) {
        itemList.Add(item);
    }

    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
    }

    public List<Item> GetItemList() {
        return itemList;
    }

}
