using UnityEngine;
using UnityEngine.Tilemaps;
using Dungeon.UI;
using Dungeon.MapSystem;
using Dungeon.Spawning;
using Dungeon.Creatures;

namespace Dungeon.Variables
{
    public class Statics : MonoBehaviour
    {
        public static GridOverlay GridOverlay;
        public static MapManager MapManager;
        public static TileDictionary TileDictionary;
        public static GameObject UI;
        public static UIManager UIManager;
        public static TimeManager TimeManager;
        public static GameObject MenuResources;
        public static DayNightManager DayNightManager;
        public static GameObject EventSystem;
        public static Tilemap TileMapFG;
        public static Tilemap TileMapBG;
        public static SelectionBox SelectionBox;
        public static InventoryUI inventoryUI;
        public static CreatureSpawner creatureSpawner;
        void Awake()
        {
            GridOverlay = Camera.main.GetComponent<GridOverlay>();
            UI = GameObject.Find("UI");
            EventSystem = GameObject.Find("EventSystem");
            MapManager = EventSystem.GetComponent<MapManager>();
            TileDictionary = GameObject.Find("MapSystem").GetComponent<TileDictionary>();
            UIManager = UI.GetComponent<UIManager>();
            TimeManager = EventSystem.GetComponent<TimeManager>();
            DayNightManager = GameObject.Find("GameSystem").GetComponent<DayNightManager>();
            TileMapBG = GameObject.Find("TilemapBG").GetComponent<Tilemap>();
            TileMapFG = GameObject.Find("TilemapFG").GetComponent<Tilemap>();
            SelectionBox = EventSystem.GetComponent<SelectionBox>();
            inventoryUI = UI.GetComponent<InventoryUI>();
            creatureSpawner = EventSystem.GetComponent<CreatureSpawner>();
        }
    }
}
