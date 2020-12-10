using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class MenuUITab : BaseUITab
    {
        private List<Button> MenuButtons;
        private enum Menu
        {
            Exit,
            Save,
            Load,
            Restart,
        }
        public void Start()
        {
            MenuButtons = new List<Button>()
            {
                gameObject.transform.Find("MenuTab").Find("ButtonExit").gameObject.GetComponent<Button>(),
                gameObject.transform.Find("MenuTab").Find("ButtonSave").gameObject.GetComponent<Button>(),
                gameObject.transform.Find("MenuTab").Find("ButtonLoad").gameObject.GetComponent<Button>(),
                gameObject.transform.Find("MenuTab").Find("ButtonRestart").gameObject.GetComponent<Button>()
            };
            for (int b = 0; b < MenuButtons.Count; b++)
            {
                int x = b;
                Button btn = MenuButtons[x];
                btn.onClick.AddListener(() => ButtonMenuCallBack(x));
            }
            Variables.Statics.GridOverlay.showMain = false;
            Variables.Statics.GridOverlay.showSub = false;
        }

        public override void SetInactive()
        {
            base.SetInactive();
        }

        protected override void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace)
        {
            
        }

        protected override void OnItemSelected(Button button, BaseUIElementHolder item)
        {

        }

        private void ButtonMenuCallBack(int buttonPressed)
        {
            switch (buttonPressed)
            {
                case (int)Menu.Exit:
                    Application.Quit();
                    break;
                case (int)Menu.Save:
                    Variables.Statics.MapManager.SaveMap();
                    break;
                case (int)Menu.Load:
                    Variables.Statics.MapManager.LoadMap("new_test_map.json");
                    break;
                case (int)Menu.Restart:
                    SceneManager.LoadScene("Demo");
                    break;
            }
        }
    }
}

