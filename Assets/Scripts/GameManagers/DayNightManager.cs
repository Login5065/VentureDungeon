using DG.Tweening;
using Dungeon.Creatures;
using Dungeon.Spawning;
using Dungeon.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Dungeon.Variables
{
    public class DayNightManager : MonoBehaviour
    {
        public bool IsNight { get; private set; } = true;
        public bool IsDay => !IsNight;
        public bool CanEndDay => !CreatureManager.register.objects.Where(x => (x.Value.allegiance && !x.Value.dying)).Any() && CreatureSpawner.done;
        public bool CanEndNight => !CreatureManager.register.objects.Where(x => (x.Value.allegiance && !x.Value.dying)).Any();
        private int heroSpawnCount;
        public float DayProgress => IsNight ? 0 : (float)(heroSpawnCount - CreatureManager.register.objects.Where(x => x.Value.allegiance).Count()) / heroSpawnCount;
        public int DaysPassed { get; private set; }
        public Transform messageEndDay;
        public Transform messageEndNight;
        private Vignette vignette;
        List<string> endDayQuotes;
        List<string> endNightQuotes;
        List<Creature> toClean;
        public void Start()
        {
            messageEndDay = Instantiate(Resources.Load<GameObject>("UI/MessageEndDay"), Statics.UI.transform, false).transform;
            messageEndNight = Instantiate(Resources.Load<GameObject>("UI/MessageEndNight"), Statics.UI.transform, false).transform;
            toClean = new List<Creature>();
            Camera.main.GetComponent<Volume>().profile.TryGet<Vignette>(out vignette);
            endDayQuotes = new List<string>()
            {
                "Cleaning Up Corpses",
                "Giving Skeletons Milk",
                "Polishing Armor",
                "Sweeping Floors",
                "Counting Coin",
                "Petting Dragons",
                "Unbreaking Pots",
                "Unhiding Kobolds",
                "Paying Out Salaries",
                "Closing The Gates"
            };
            endNightQuotes = new List<string>()
            {
                "Heroes Are Scheming",
                "Time's Up, Let's Do This",
                "Heroes Heading Out",
                "Opening the Gates",
                "Hiding Kobolds",
                "Putting Out Posters",
                "Ending Maintenance",
                "Waking Up Dragons",
                "Heroes Downloading Updates",
                "Unstucking Dungeon Entry"
            };
        }

        public IEnumerator SetNight()
        {
            if (IsNight) yield break;
            StartCoroutine(ActivateGate());
            yield return new WaitForSecondsRealtime(1.5f);
            messageEndDay.Find("Text").GetComponent<Text>().text = endDayQuotes[Random.Range(0, endDayQuotes.Count)];
            messageEndDay.DOLocalMoveY(490, 1, true).SetUpdate(true);
            IsNight = true;
            vignette.intensity.value = 0.4f;
            TimeManager.DayText.text = "Night " + DaysPassed;
            toClean.Clear();
            foreach (var creature in CreatureManager.register.objects.Values)
            {
                if (creature.Health <= 0 || creature.dying || creature.allegiance)
                    toClean.Add(creature);
                else
                    creature.health = creature.maxHealth;
            }
            for (int i = 0; i < toClean.Count; i++)
            {
                CreatureManager.CleanCreature(toClean[i]);
            }
            yield return new WaitForSecondsRealtime(4.0f);
            messageEndDay.DOLocalMoveY(591, 1, true).SetUpdate(true);
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(DeactivateGate());
            yield return new WaitForSecondsRealtime(2.0f);
            Statics.TimeManager.periodEnded = false;
            yield break;
        }

        public IEnumerator SetDay(int totalHeroSpawnCount)
        {
            if (IsDay) yield break;
            StartCoroutine(ActivateGate());
            yield return new WaitForSecondsRealtime(1.5f);
            messageEndNight.Find("Text").GetComponent<Text>().text = endNightQuotes[Random.Range(0, endNightQuotes.Count)];
            messageEndNight.DOLocalMoveY(490, 1, true).SetUpdate(true);
            IsNight = false;
            vignette.intensity.value = 0;
            FameInterface.ListHeroesToSpawn();
            DaysPassed++;
            TimeManager.DayText.text = "Day " + DaysPassed;
            yield return new WaitForSecondsRealtime(4.0f);
            messageEndNight.DOLocalMoveY(591, 1, true).SetUpdate(true);
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(DeactivateGate());
            yield return new WaitForSecondsRealtime(2.0f);
            Statics.TimeManager.periodEnded = false;
            Statics.creatureSpawner.StartSpawning();
            yield break;
        }

        public IEnumerator ActivateGate()
        {
            Statics.UIManager.Gate.GetComponent<AudioSource>().Play();
            yield return new WaitForSecondsRealtime(0.5f);
            Statics.UIManager.Gate.transform.Find("GateLeft").transform.DOLocalMoveX(0, 1.5f).SetUpdate(true);
            Statics.UIManager.Gate.transform.Find("GateRight").transform.DOLocalMoveX(0, 1.5f).SetUpdate(true);
            yield break;
        }

        public IEnumerator DeactivateGate()
        {
            Statics.UIManager.Gate.GetComponent<AudioSource>().Play();
            yield return new WaitForSecondsRealtime(0.5f);
            Statics.UIManager.Gate.transform.Find("GateLeft").transform.DOLocalMoveX(-960, 1.5f).SetUpdate(true);
            Statics.UIManager.Gate.transform.Find("GateRight").transform.DOLocalMoveX(960, 1.5f).SetUpdate(true);
            yield break;
        }
    }
}