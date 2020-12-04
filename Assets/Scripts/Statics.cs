using UnityEngine;
using UnityEngine.Tilemaps;
using Dungeon.UI;
using Dungeon.MapSystem;

namespace Dungeon.Variables
{
    public class Statics : MonoBehaviour
    {
        public static GridOverlay GridOverlay;
        public static MapManager MapManager;
        public static TileDictionary TileDictionary;
        public static GameObject UI;
        public static UIManager UIManager;
        public static ArchitectUIManager ArchitectUIManager;
        public static GameObject LeftMenuArchitect;
        public static MonstersUIManager MonstersUIManager;
        public static GameObject LeftMenuObjects;
        public static ObjectsUIManager ObjectsUIManager;
        public static GameObject LeftMenuMonsters;
        public static ResourcesUIManager ResourcesUIManager;
        public static GameObject MenuResources;
        public static GameObject EventSystem;
        public static TileSelector TileSelector;
        public static Tilemap TileMapFG;
        public static Tilemap TileMapBG;
        public static SelectionBox SelectionBox;
        public static InventoryUI inventoryUI;
        public static GameObject LeftMenuInventory;
        void Awake()
        {
            GridOverlay = Camera.main.GetComponent<GridOverlay>();
            UI = GameObject.Find("UI");
            EventSystem = GameObject.Find("EventSystem");
            MapManager = EventSystem.GetComponent<MapManager>();
            TileDictionary = GameObject.Find("MapSystem").GetComponent<TileDictionary>();
            UIManager = UI.GetComponent<UIManager>();
            ArchitectUIManager = UI.GetComponent<ArchitectUIManager>();
            LeftMenuArchitect = UI.transform.Find("LeftMenuArchitect").gameObject;
            MonstersUIManager = UI.GetComponent<MonstersUIManager>();
            LeftMenuMonsters = UI.transform.Find("LeftMenuMonsters").gameObject;
            ObjectsUIManager = UI.GetComponent<ObjectsUIManager>();
            LeftMenuObjects = UI.transform.Find("LeftMenuObjects").gameObject;
            ResourcesUIManager = UI.transform.Find("Resources").GetComponent<ResourcesUIManager>();
            MenuResources = UI.transform.Find("Resources").gameObject;
            TileSelector = EventSystem.GetComponent<TileSelector>();
            TileMapBG = GameObject.Find("TilemapBG").GetComponent<Tilemap>();
            TileMapFG = GameObject.Find("TilemapFG").GetComponent<Tilemap>();
            SelectionBox = EventSystem.GetComponent<SelectionBox>();
            inventoryUI = UI.GetComponent<InventoryUI>();
            LeftMenuInventory = UI.transform.Find("LeftMenuInventory").gameObject;
        }
    }
}
