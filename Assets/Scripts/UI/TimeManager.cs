using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Dungeon.Variables;
using DG.Tweening;

namespace Dungeon.UI
{
    public class TimeManager : MonoBehaviour
    {
        private readonly List<Button> timeButtons = new List<Button>();
        public GameObject paused;
        public GameObject timeUI;
        public static Text DayText;
        private Button buttonEndDay;
        private Button buttonEndNight;
        private bool endDayReleased = false;
        private bool endNightReleased = false;
        public bool periodEnded = false;

        private void Start()
        {
            paused = Instantiate(Resources.Load<GameObject>("UI/Paused"), Statics.UI.transform, false);
            timeUI = Instantiate(Resources.Load<GameObject>("UI/Time"), Statics.UI.transform, false);
            timeButtons.Add(timeUI.transform.Find("ButtonPause").GetComponent<Button>());
            timeButtons.Add(timeUI.transform.Find("ButtonPlay").GetComponent<Button>());
            timeButtons.Add(timeUI.transform.Find("ButtonFast").GetComponent<Button>());
            timeButtons.Add(timeUI.transform.Find("ButtonZoom").GetComponent<Button>());
            buttonEndDay = timeUI.transform.Find("ButtonEndDay").GetComponent<Button>();
            buttonEndNight = timeUI.transform.Find("ButtonEndNight").GetComponent<Button>();
            DayText = timeUI.transform.Find("Clocc").Find("Day").GetComponent<Text>();
            paused.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                var x = i;
                var button = timeButtons[x];
                button.onClick.AddListener(() => ChangeSpeed(x));
            }
            buttonEndDay.onClick.AddListener(() => EndDay());
            buttonEndNight.onClick.AddListener(() => EndNight());
        }

        private void Update()
        {
            if (periodEnded) return;
            if (endDayReleased && !Statics.DayNightManager.CanEndDay)
            {
                endDayReleased = false;
                buttonEndDay.transform.DOLocalMoveY(-239, 1.5f, true).SetUpdate(true);
            }
            if (endNightReleased && !Statics.DayNightManager.CanEndNight)
            {
                endNightReleased = false;
                buttonEndNight.transform.DOLocalMoveY(-239, 1.5f, true);
            }
            if (Statics.DayNightManager.IsDay)
            {
                if (!endDayReleased && Statics.DayNightManager.CanEndDay)
                {
                    endDayReleased = true;
                    buttonEndDay.transform.DOLocalMoveY(-769, 1.5f, true).SetUpdate(true);
                }
            }
            else
            {
                if (!endNightReleased && Statics.DayNightManager.CanEndNight)
                {
                    endNightReleased = true;
                    buttonEndNight.transform.DOLocalMoveY(-769, 1.5f, true).SetUpdate(true);
                }
            }
        }

        private void ChangeSpeed(float x)
        {
            if (x == 0)
            {
                paused.SetActive(true);
            }
            else
            {
                paused.SetActive(false);
            }
            Time.timeScale = x;
        }

        private void EndDay()
        {
            if (periodEnded) return;
            endDayReleased = false;
            periodEnded = true;
            buttonEndDay.transform.DOLocalMoveY(-239, 1.5f, true).SetUpdate(true);
            StartCoroutine(Statics.DayNightManager.SetNight());
        }

        private void EndNight()
        {
            if (periodEnded) return;
            endNightReleased = false;
            periodEnded = true;
            buttonEndNight.transform.DOLocalMoveY(-239, 1.5f, true).SetUpdate(true);
            StartCoroutine(Statics.DayNightManager.SetDay(5));
        }
    }
}
