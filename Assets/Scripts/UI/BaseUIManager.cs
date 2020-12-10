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
        private const int Width = 2;
        private const int Height = 5;
        private const int ElementsPerPage = Width * Height;
        #endregion

        #region Debug stuff
        protected bool displayDebugElements;
        public bool DisplayDebugElements
        {
            set
            {
                displayDebugElements = value;
                if (!displayDebugElements && currentPage > MaxPage)
                    currentPage = MaxPage;
            }
        }
        public int currentPage;
        #endregion

        #region UI
        public List<BaseUIElement> elements;
        public BaseUIElementHolder[] mainButtons;
        public Button buttonBack;
        public Button buttonForward;
        public Text currentPageText;
        public Text maxPageText;
        public GameObject scrollOutline;
        protected BaseUIElementHolder currentElement;
        protected int currentData;
        public GameObject shopItem;
        public float startX = -1279;
        public float endX = -710;
        public bool selected = false;
        public bool active = false;
        public virtual void SetupTab()
        {

        }

        // TODO: only calculate those values on startup and once Elements is changed
        public int MaxPageDebug => elements.Count / ElementsPerPage;
        public int MaxPage => elements.Where(x => !x.IsDebug).Count() / ElementsPerPage;
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
            currentElement = null;
        }
        #endregion

        #region Methods that might not be needed to implement/override

        /// <summary> Method used to hook this script with buttons inside of the tab</summary>
        public virtual void GenerateUIPage()
        {
            mainButtons = new BaseUIElementHolder[ElementsPerPage];
            shopItem = Resources.Load<GameObject>("UI/ShopItem");
            int currentElement = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var newButton = Instantiate(shopItem, gameObject.transform);
                    newButton.transform.localPosition = new Vector3(-110.0f + 220.0f * x, 256.0f - 128.0f * y, 0);
                    var button = newButton.GetComponent<Button>();
                    var uiElementHolder = newButton.GetComponent<BaseUIElementHolder>();
                    mainButtons[currentElement++] = uiElementHolder;

                    button.onClick.AddListener(() => OnItemSelected(button, uiElementHolder));
                }
            }

            buttonBack = gameObject.transform.Find("PageUp").GetComponent<Button>();
            buttonBack.onClick.AddListener(OnBackButtonClicked);

            buttonForward = gameObject.transform.Find("PageDown").GetComponent<Button>();
            buttonForward.onClick.AddListener(OnForwardButtonClicked);

            currentPageText = gameObject.transform.Find("CurrentPage").GetComponent<Text>();
            maxPageText = gameObject.transform.Find("MaxPage").GetComponent<Text>();
            scrollOutline = gameObject.transform.Find("ScrollOutline").gameObject;
        }

        /// <summary>Method that should be called every time data in the tab changes, like the current page</summary>
        public virtual void RefreshUIPageData()
        {
            var check = (displayDebugElements && currentPage < MaxPageDebug) || currentPage < MaxPage;
            var check2 = (displayDebugElements && MaxPageDebug < 2) || MaxPage < 2;
            buttonBack.gameObject.SetActive(currentPage > 0 && !check2);
            buttonForward.gameObject.SetActive(check && !check2);
            currentPageText.gameObject.SetActive(!check2);
            maxPageText.gameObject.SetActive(!check2);
            scrollOutline.SetActive(!check2);

            var elementOffset = ElementsPerPage * currentPage;

            for (int i = 0; i < mainButtons.Length; i++)
            {
                var element = mainButtons[i];
                var elementPos = i + elementOffset;

                element.gameObject.SetActive(false);
                if (elementPos < elements.Count)
                {
                    var currentElement = elements[elementPos];
                    if (!currentElement.IsDebug || displayDebugElements)
                    {
                        element.gameObject.SetActive(true);
                        element.element = currentElement;
                        element.UpdateData();
                    }
                }
            }
        }

        public virtual void OnUpdate()
        {
            if (currentElement != null)
            {
                var click = Input.GetMouseButtonDown(0) && !UI.MouseInputUIBlocker.BlockedByUI;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                OnItemUpdate(mousePos, click);
            }
        }
        #endregion

        #region Back/forward buttons
        /// <summary>Called by back button, moved to separate method instead of lambda in case child class needs to change this functionality</summary>
        protected virtual void OnBackButtonClicked()
        {
            if (currentPage > 0)
            {
                currentPage--;
                RefreshUIPageData();
            }
        }

        /// <summary>Called by forward button, moved to separate method instead of lambda in case child class needs to change this functionality</summary>
        protected virtual void OnForwardButtonClicked()
        {
            if ((displayDebugElements && currentPage < MaxPageDebug) || currentPage < MaxPage)
            {
                currentPage++;
                RefreshUIPageData();
            }
        }
        #endregion
    }
    #endregion
}