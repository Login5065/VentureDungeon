using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.Variables;
using System;

namespace Dungeon.UI
{
    public class ArchitectUIManager : MonoBehaviour
    {
        private readonly List<Button> ArchitectButtons = new List<Button>();
        public int material = 0;
        public bool ground = true;

        void Start()
        {
            foreach (var item in Enum.GetNames(typeof(Register.TileTypes)))
            {
                ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("Button" + item).GetComponent<Button>());
            }
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonMine").GetComponent<Button>());
            for (int b = 0; b < ArchitectButtons.Count; b++) {
                int x = b;
                Button btn = ArchitectButtons[x];
                btn.onClick.AddListener(() => ButtonArchitectCallBack(x));
            }
            material = (int)UIManager.UIModes.None;
            Statics.LeftMenuArchitect.transform.Find("GroundSwitchNub").Find("GroundSwitch").gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonGroundSwitchCallBack());
        }

        private void ButtonArchitectCallBack(int buttonPressed)
        {
            Statics.UIManager.mode = (int)UIManager.UIModes.Build;
            material = buttonPressed;
        }

        private void ButtonGroundSwitchCallBack()
        {
            ground = !ground;
            if (ground)
            {
                Statics.LeftMenuArchitect.transform.Find("GroundSwitchNub").Find("GroundSwitch").Find("Text").GetComponent<Text>().text = "Foreground";
            }
            else
            {
                Statics.LeftMenuArchitect.transform.Find("GroundSwitchNub").Find("GroundSwitch").Find("Text").GetComponent<Text>().text = "Background";
            }
        }
    }
}

