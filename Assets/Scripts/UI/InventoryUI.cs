using System.Collections.Generic;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using Dungeon.Creatures;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Dungeon.MapSystem;

namespace Dungeon.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public Inventory inventory;
        //private Button InventoryTemplate;
        private GameObject InventoryContainer;
        private int z;
        private List<Button> InventoryList;
        private float checkDelay = 0;
        private Item currItem = null;
        public GameObject itemSelected;
        public GameObject itemSelectedGhost;
        public GameObject itemGhost;


        private void Start()
        {
            InventoryList = new List<Button>();
            inventory = new Inventory();
            InventoryContainer = Statics.LeftMenuInventory;
            RefreshInventoryItems();
            z = 0;

        }

        public void RefreshInventoryItems()
        {
            int x = 0, y = 0;
            z = 0;
            float slotWidht = 150f;
            float slotHeight = 175f;

            for (int i = InventoryList.Count - 1; i >= 0; i--)
            {
                Destroy(InventoryList[i].gameObject);
                InventoryList.RemoveAt(i);

            }

            //InventoryList.Clear();

            foreach (Item item in inventory.GetItemList())
            {

                InventoryList.Add(Instantiate(Resources.Load<Button>("Items/ItemTemplate"), InventoryContainer.transform).GetComponent<Button>());
                InventoryList[z].gameObject.SetActive(true);
                InventoryList[z].transform.position = new Vector2(+x * slotWidht + 70, -y * slotHeight + 670);
                InventoryList[z].image.sprite = item.GetSprite();
                x++;

                if (x >= 3)
                {

                    x = 0; y++;
                }
                InventoryList[z].GetComponent<Button>().onClick.AddListener(delegate { TaskOnClick(item,z); });
                z++;


            }
        }

        void TaskOnClick(Item item, int x)
        {

            Destroy(itemGhost);
            itemSelected = null;
            itemSelectedGhost = null;
            itemGhost = null;
            itemSelected = Resources.Load<GameObject>("Items/ItemObject_pf");
            itemSelectedGhost = Resources.Load<GameObject>("Items/ItemObjectGhost_pf");
            itemSelected.GetComponent<SpriteRenderer>().sprite = item.GetSprite();
            currItem = item;
            if (itemSelected != null)
            {
                Statics.UIManager.mode = (int)UIManager.UIModes.Inventory;
                
            }
        }

        private void Update()
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.Inventory)
            {
                if (itemSelected != null)
                {
                    var click = Input.GetMouseButtonDown(0);
                    if ((click || checkDelay <= 0) && !UI.MouseInputUIBlocker.BlockedByUI)
                    {
                        HandlePlacement(click);
                        checkDelay = 0.05f;
                    }
                    else checkDelay -= Time.unscaledDeltaTime;
                }
                else Statics.UIManager.mode = (int)UIManager.UIModes.None;
            }
            else
            {
                
                itemSelected = null;
                if (itemGhost != null)
                {
                    Destroy(itemGhost);
                    itemGhost = null;
                }
            }
        }

        private void HandlePlacement(bool place)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var tilePos = mousePos;
            var tile = tilePos;
            HandleGhost(tilePos, true);
            if (place)
            {
                if (currItem != null)
                {
                    ItemWorld.PlaceItemWorld(new Vector3(tilePos.x, tilePos.y, tilePos.z), currItem);
                    inventory.RemoveItem(currItem);
                    RefreshInventoryItems();
                }
                currItem = null;
            }
            else HandleGhost(tilePos, false);
        }

        private void HandleGhost(Vector3Int tilePos, bool canPlace)
            => HandleGhost(tilePos + new Vector3(0.5f, 0), canPlace);

        private void HandleGhost(Vector3 pos, bool canPlace)
        {
            if (itemGhost == null)
            {
                itemGhost = Instantiate(Resources.Load<GameObject>("Items/ItemObjectGhost_pf"), Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            }

            itemGhost.transform.position = pos;
            if (canPlace)
                itemGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 0.5f);
            else
                itemGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 0.5f); ;
        }







    }
}