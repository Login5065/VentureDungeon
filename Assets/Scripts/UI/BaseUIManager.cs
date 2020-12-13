using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.UI
{

    #region Tab
    public abstract class BaseUITab : MonoBehaviour
    {
        #region Constants
        //private const int Width = 2;
        //private const int Height = 5;
        //private const int ElementsPerPage = Width * Height;
        #endregion

        #region Debug stuff
        protected bool displayDebugElements;
        public bool DisplayDebugElements
        {
            set => displayDebugElements = value;
        }
        #endregion

        #region UI
        public List<BaseUIElement> elements;
        public List<BaseUIElementHolder> mainButtons;
        protected BaseUIElementHolder currentElement;
        protected int currentData;
        public GameObject shopItem;
        public float startX = -1279;
        public float endX = -710;
        public bool selected = false;
        public bool active = false;
        public int items = 0;
        public virtual void SetupTab()
        {

        }

        // TODO: only calculate those values on startup and once Elements is changed
        //public int MaxPageDebug => elements.Count / ElementsPerPage;
        //public int MaxPage => elements.Where(x => !x.IsDebug).Count() / ElementsPerPage;
        #endregion

        #region Main methods to implement/override
        /// <summary>Called any time an element selecting, non-special button is called. Base implementation.</summary>
        /// <param name="button">Button that was clicked, in case it's needed for anything</param>
        /// <param name="item">The element holder attached to the button</param>
        protected virtual void OnItemSelected(Button button, BaseUIElementHolder item)
        {
            currentElement = item;
        }

        protected abstract void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace);
        public virtual void SetActive()
        {
            if (active) return;
            active = true;
            transform.DOLocalMoveX(endX, 1, true);
        }
        public virtual void SetInactive()
        {
            if (!active) return;
            active = false;
            transform.DOLocalMoveX(startX, 1, true);
            CursorManager.SetCursor("White");
            currentElement = null;
        }
        #endregion

        #region Methods that might not be needed to implement/override

        /// <summary> Method used to hook this script with buttons inside of the tab</summary>
        public virtual void GenerateUIPage(int items)
        {
            mainButtons = new List<BaseUIElementHolder>();
            shopItem = Resources.Load<GameObject>("UI/ShopItem");
            for (int y = 0; y < items; y++)
            {
                var newButton = Instantiate(shopItem, gameObject.transform.Find("Scroll View").Find("Viewport").Find("Content"));
                var button = newButton.GetComponent<Button>();
                var uiElementHolder = newButton.GetComponent<BaseUIElementHolder>();
                mainButtons.Add(uiElementHolder);

                button.onClick.AddListener(() => OnItemSelected(button, uiElementHolder));
            }
            var rect = gameObject.transform.Find("Scroll View");
            //if (rect != null) rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors;
        }

        /// <summary>Method that should be called every time data in the tab changes, like the current page</summary>
        public virtual void RefreshUIPageData()
        {
            for (int i = 0; i < mainButtons.Count; i++)
            {
                var element = mainButtons[i];
                element.gameObject.SetActive(false);
                if (!elements[i].IsDebug || displayDebugElements)
                {
                    element.gameObject.SetActive(true);
                    element.element = elements[i];
                    element.UpdateData();
                }
            }
        }

        public virtual void OnUpdate()
        {
            if (currentElement != null)
            {
                var click = Input.GetMouseButtonDown(0) && !MouseInputUIBlocker.BlockedByUI;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                OnItemUpdate(mousePos, click);
            }
        }
        #endregion
    }
    #endregion
}