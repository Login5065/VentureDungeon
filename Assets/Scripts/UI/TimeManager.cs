using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Dungeon.Variables;

namespace Dungeon.UI
{
    public class TimeManager : MonoBehaviour
    {
        private readonly List<Button> timeButtons = new List<Button>();

        private void Start()
        {
            timeButtons.Add(GameObject.Find("ButtonPause").GetComponent<Button>());
            timeButtons.Add(GameObject.Find("ButtonPlay").GetComponent<Button>());
            timeButtons.Add(GameObject.Find("ButtonFast").GetComponent<Button>());
            timeButtons.Add(GameObject.Find("ButtonZoom").GetComponent<Button>());
            Statics.UI.transform.Find("Paused").gameObject.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                var x = i;
                var button = timeButtons[x];
                button.onClick.AddListener(() => ChangeSpeed(x));
            }
        }

        private void ChangeSpeed(float x)
        {
            if (x == 0)
            {
                Statics.UI.transform.Find("Paused").gameObject.SetActive(true);
            }
            else
            {
                Statics.UI.transform.Find("Paused").gameObject.SetActive(false);
            }
            Time.timeScale = x;
        }
    }
}
