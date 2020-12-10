
/* Jacek Kapelańczyk
 This class works as simple global data storage
 
 To use this class create empty object in first scene and just set it there 
 change data if needed
 */

using UnityEngine;
using System.Collections;

namespace Dungeon.Variables
{
    public class GameData : MonoBehaviour
    {
        public static int gold = 1000;
        public static int Gold
        {
            get => gold;
            set
            {
                gold = value;
                UI.ResourcesUIManager.GoldText.text = gold.ToString();
            }
        }
        public static int fame = 50;
        public static int Fame
        {
            get => fame;
            set
            {
                if (value < 0) fame = 0;
                else fame = value;
                UI.ResourcesUIManager.FameText.text = fame.ToString();
                Creatures.CreatureSpawner.mainSpawnCooldown = 3000/fame;
            }
        }
        public static int threat = 0;
        public static int Threat
        {
            get => threat;
            set
            {
                if (value < 0) threat = 0;
                else threat = value;
                UI.ResourcesUIManager.ThreatText.text = threat.ToString();
            }
        }
        public static int monsterIndex = 0;
        public static int monsterID
        {
            get
            {
                monsterIndex++;
                return monsterIndex;
            }
        }
        void Start()
        {
            StartCoroutine(TickDownFame());
        }
        IEnumerator TickDownFame()
        {
            yield return new WaitForSeconds(60.0f);
            while (true)
            {
                if (Statics.DayNightManager.IsDay)
                {
                    Fame -=1;
                }
                yield return new WaitForSeconds(60.0f);
            }
        }
    }
}

