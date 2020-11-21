using UnityEngine;

namespace Dungeon.Objects
{
    public class Treasure : PlaceableObject
    {
        public int currentGold;
        public int maxGold;
        public override bool CanSell => true;
        public override int GoldValue => currentGold;
        void Start()
        {
            Variables.GameData.gameStarted = true;
        }
    }
}
