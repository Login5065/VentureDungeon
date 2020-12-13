using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.Creatures;
using DG.Tweening;
using DevionGames.UIWidgets;

namespace Dungeon.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private bool displayDebugElements = false;
        [SerializeField]
        public List<BaseUITab> UITabs;
        public List<Button> UIButtons;
        public BaseUITab activeUITab;
        public Creature SelectedCreature;
        public GameObject ResourcesUI;
        public GameObject Gate;
        public Tooltip Tooltip;
        public bool DisplayDebugElements
        {
            get => displayDebugElements;
            set
            {
                if (displayDebugElements != value)
                {
                    displayDebugElements = value;
                    activeUITab.DisplayDebugElements = displayDebugElements;
                }
            }
        }
        public enum UIModes 
        {
            None,
            Move,
            Build,
            Monster,
            Object,
            Inventory
        }
        public int mode = 0;

        void Start()
        {
            UITabs = new List<BaseUITab>()
            {
                Instantiate(Resources.Load<GameObject>("UI/ArchitectTab"), gameObject.transform, false).GetComponent<ArchitectUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/MonstersTab"), gameObject.transform, false).GetComponent<MonstersUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/ObjectsTab"), gameObject.transform, false).GetComponent<ObjectsUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/PowersTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/ResearchTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/HeroesTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/StatsTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/MenuTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
                Instantiate(Resources.Load<GameObject>("UI/InventoryTab"), gameObject.transform, false).GetComponent<EmptyUITab>(),
            };
            UIButtons = new List<Button>()
            {
                Instantiate(Resources.Load<GameObject>("UI/ButtonArchitect"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonMonsters"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonObjects"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonPowers"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonResearch"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonHeroes"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonStats"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonMenu"), gameObject.transform, false).GetComponent<Button>(),
                Instantiate(Resources.Load<GameObject>("UI/ButtonInventory"), gameObject.transform, false).GetComponent<Button>(),
            };
            ResourcesUI = Instantiate(Resources.Load<GameObject>("UI/Resources"), gameObject.transform, false);
            Gate = Instantiate(Resources.Load<GameObject>("UI/Gate"), gameObject.transform, false);
            Tooltip = Instantiate(Resources.Load<GameObject>("UI/Tooltip"), gameObject.transform, false).GetComponent<Tooltip>();
            for (int i = 0; i < UITabs.Count; i++)
            {
                var x = i;
                UITabs[x].SetupTab();
                UITabs[x].GenerateUIPage(UITabs[x].elements.Count);
                UIButtons[x].onClick.AddListener(() => ButtonBottomUICallBack(x));
                UITabs[x].gameObject.transform.DOLocalMoveX(UITabs[x].startX, 0, true);
            }
        }

        private void ButtonBottomUICallBack(int buttonPressed)
        {
            if (activeUITab != null) activeUITab.SetInactive();
            Variables.Statics.GridOverlay.showMain = false;
            Variables.Statics.GridOverlay.showSub = false;
            activeUITab = UITabs[buttonPressed];
            activeUITab.DisplayDebugElements = DisplayDebugElements;
            activeUITab.RefreshUIPageData();
            activeUITab.SetActive();
        }

        void Update()
        {
            if (activeUITab != null) activeUITab.OnUpdate();
            if (Input.GetMouseButton(1) && mode != (int)UIModes.Move) {
                foreach (BaseUITab tab in UITabs)
                {
                    tab.SetInactive();
                }
                Variables.Statics.GridOverlay.showMain = false;
                Variables.Statics.GridOverlay.showSub = false;
                if (SelectedCreature != null)
                {
                    SelectedCreature = null;
                }
                mode = (int)UIModes.None;
            }
        }
    }
}