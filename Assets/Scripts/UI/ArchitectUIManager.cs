using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.Variables;

namespace Dungeon.UI
{
    public class ArchitectUIManager : MonoBehaviour
    {
        private readonly List<Button> ArchitectButtons = new List<Button>();
        public enum Architect
        {
            Wood,
            StoneBrick,
            Glass,
            Ladder,
            Oak,
            Birch,
            Brick,
            CottonBlue,
            CottonGreen,
            CottonRed,
            Cotton,
            Mine,
            None,
        }
        public int material = 0;
        public bool ground = true;
        // Start is called before the first frame update
        void Start()
        {
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonWood").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonStoneBrick").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonGlass").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonLadder").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonOak").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonBirch").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonBrick").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonBlueCotton").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonGreenCotton").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonRedCotton").GetComponent<Button>());
            ArchitectButtons.Add(Statics.LeftMenuArchitect.transform.Find("ButtonCotton").GetComponent<Button>());
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

