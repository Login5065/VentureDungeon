using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dungeon.Creatures;

namespace Dungeon.UI
{
    public class UIManager : MonoBehaviour
    {
        public static GameObject LeftMenuOuter;
        public static Button ArchitectButton;
        public static GameObject LeftMenuArchitect;
        public static Button MonstersButton;
        public static GameObject LeftMenuMonsters;
        public static Button ObjectsButton;
        public static GameObject LeftMenuObjects;
        public static Button PowersButton;
        public static GameObject LeftMenuPowers;
        public static Button ResearchButton;
        public static GameObject LeftMenuResearch;
        public static Button HeroesButton;
        public static GameObject LeftMenuHeroes;
        public static Button StatsButton;
        public static GameObject LeftMenuStats;
        public static Button MenuButton;
        public static GameObject RightMenu;
        private List<Button> BottomUIButtons = new List<Button>();
        private List<Button> MenuButtons = new List<Button>();
        private List<GameObject> BottomUITabs = new List<GameObject>();
        public Creature SelectedCreature;
        public static Text SelectedName;
        private enum BottomUI 
        {
            Architect,
            Monsters,
            Objects,
            Powers,
            Research,
            Heroes,
            Stats,
            Menu
        }
        private enum Menu
        {
            Exit,
            Save,
            Load,
            Restart,
        }
        public enum UIModes 
        {
            None,
            Move,
            Build,
            Monster,
            Object,
        }
        public int mode = 0;

        void Start()
        {
            LeftMenuOuter = GameObject.Find("LeftMenuOuter");
            MenuButton = GameObject.Find("ButtonMenu").GetComponent<Button>();
            BottomUIButtons.Add(ArchitectButton = GameObject.Find("ButtonArchitect").GetComponent<Button>());
            BottomUIButtons.Add(MonstersButton = GameObject.Find("ButtonMonsters").GetComponent<Button>());
            BottomUIButtons.Add(ObjectsButton = GameObject.Find("ButtonObjects").GetComponent<Button>());
            BottomUIButtons.Add(PowersButton = GameObject.Find("ButtonPowers").GetComponent<Button>());
            BottomUIButtons.Add(ResearchButton = GameObject.Find("ButtonResearch").GetComponent<Button>());
            BottomUIButtons.Add(HeroesButton = GameObject.Find("ButtonHeroes").GetComponent<Button>());
            BottomUIButtons.Add(StatsButton = GameObject.Find("ButtonStats").GetComponent<Button>());
            BottomUIButtons.Add(MenuButton = GameObject.Find("ButtonMenu").GetComponent<Button>());
            BottomUITabs.Add(LeftMenuArchitect = GameObject.Find("LeftMenuArchitect"));
            BottomUITabs.Add(LeftMenuMonsters = GameObject.Find("LeftMenuMonsters"));
            BottomUITabs.Add(LeftMenuObjects = GameObject.Find("LeftMenuObjects"));
            BottomUITabs.Add(LeftMenuPowers = GameObject.Find("LeftMenuPowers"));
            BottomUITabs.Add(LeftMenuResearch = GameObject.Find("LeftMenuResearch"));
            BottomUITabs.Add(LeftMenuHeroes = GameObject.Find("LeftMenuHeroes"));
            BottomUITabs.Add(LeftMenuStats = GameObject.Find("LeftMenuStats"));
            BottomUITabs.Add(RightMenu = GameObject.Find("RightMenu"));
            MenuButtons.Add(this.gameObject.transform.Find("RightMenu").transform.Find("ButtonExit").gameObject.GetComponent<Button>());
            MenuButtons.Add(this.gameObject.transform.Find("RightMenu").transform.Find("ButtonSave").gameObject.GetComponent<Button>());
            MenuButtons.Add(this.gameObject.transform.Find("RightMenu").transform.Find("ButtonLoad").gameObject.GetComponent<Button>());
            MenuButtons.Add(this.gameObject.transform.Find("RightMenu").transform.Find("ButtonRestart").gameObject.GetComponent<Button>());
            SelectedName = this.gameObject.transform.Find("Selected").transform.Find("Name").gameObject.GetComponent<Text>();
            LeftMenuOuter.SetActive(false);
            for (int b = 0; b < MenuButtons.Count; b++) {
                int x = b;
                Button btn = MenuButtons[x];
                btn.onClick.AddListener(() => ButtonMenuCallBack(x));
            }
            for (int b = 0; b < BottomUIButtons.Count; b++) {
                int x = b;
                Button btn = BottomUIButtons[x];
                btn.onClick.AddListener(() => ButtonBottomUICallBack(x));
                BottomUITabs[x].SetActive(false);
            }
            Variables.Statics.GridOverlay.showMain = false;
            Variables.Statics.GridOverlay.showSub = false;
        }

        private void ButtonBottomUICallBack(int buttonPressed)
        {
            foreach (GameObject g in BottomUITabs)
            {
                g.SetActive(false);
            }
            Variables.Statics.GridOverlay.showMain = false;
            Variables.Statics.GridOverlay.showSub = false;
            LeftMenuOuter.SetActive(false);
            if (buttonPressed != (int) BottomUI.Menu)LeftMenuOuter.SetActive(true);
            switch (buttonPressed)
            {
                case (int) BottomUI.Architect:
                    LeftMenuArchitect.SetActive(true);
                    Variables.Statics.GridOverlay.showMain = true;
                    Variables.Statics.GridOverlay.showSub = true;
                    break;
                case (int) BottomUI.Monsters:
                    LeftMenuMonsters.SetActive(true);
                    break;
                case (int) BottomUI.Objects:
                    LeftMenuObjects.SetActive(true);
                    Variables.Statics.GridOverlay.showMain = true;
                    Variables.Statics.GridOverlay.showSub = true;
                    break;
                case (int) BottomUI.Powers:
                    LeftMenuPowers.SetActive(true);
                    break;
                case (int) BottomUI.Research:
                    LeftMenuResearch.SetActive(true);
                    break;
                case (int) BottomUI.Heroes:
                    LeftMenuHeroes.SetActive(true);
                    break;
                case (int) BottomUI.Stats:
                    LeftMenuStats.SetActive(true);
                    break;
                case (int) BottomUI.Menu:
                    RightMenu.SetActive(true);
                    break;
            }
        }

        private void ButtonMenuCallBack(int buttonPressed)
        {
            switch (buttonPressed)
            {
                case (int) Menu.Exit:
                    Application.Quit();
                    break;
                case (int) Menu.Save:
                    Variables.Statics.MapManager.SaveMap();
                    break;
                case (int) Menu.Load:
                    Variables.Statics.MapManager.LoadMap("new_test_map.json");
                    break;
                case (int) Menu.Restart:
                    SceneManager.LoadScene("Demo");
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1) && mode != (int)UIModes.Move) {
                foreach (GameObject g in BottomUITabs)
                {
                    g.SetActive(false);
                }
                Variables.Statics.GridOverlay.showMain = false;
                Variables.Statics.GridOverlay.showSub = false;
                LeftMenuOuter.SetActive(false);
                RightMenu.SetActive(false);
                if (SelectedCreature != null)
                {
                    SelectedCreature.selection.SetActive(false);
                    SelectedCreature = null;
                }
                mode = (int)UIModes.None;
            }
        }
    }
}