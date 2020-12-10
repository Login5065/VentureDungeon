using System.Collections.Generic;
using System.Linq;
using Dungeon.Creatures;
using Dungeon.Objects;
using Dungeon.Variables;
using UnityEngine;

namespace Dungeon.UI
{
    public class ObjectSellingUIManager : MonoBehaviour
    {
        public bool isInSellingMode = false;
        public bool sellMonsters = true;
        public bool sellObjects = true;

        private void Start() 
            => Statics.SelectionBox.OnSelectionFinished += SellSelectedObjects;

        private void SellSelectedObjects(RectInt rect)
        {
            if (isInSellingMode && !MouseInputUIBlocker.BlockedByUI && rect.width > 0 && rect.height > 0)
            {
                var totalValue = 0;

                if (sellObjects)
                {
                    var toSell = new List<PlaceableObject>();

                    foreach (var sellable in ObjectManager.register.Values)
                    {
                        if (sellable.CanSell && rect.Contains(sellable.GridPosition))
                            toSell.Add(sellable);
                    }

                    foreach (var sellable in toSell)
                    {
                        totalValue += sellable.GoldValue;
                        ObjectManager.KillObject(sellable);
                    }
                }

                if (sellMonsters)
                {
                    var collisions = Physics2D.OverlapAreaAll(rect.min, rect.max);

                    foreach (var creature in collisions.Where(x => x is BoxCollider2D).Select(x => x.GetComponent<Creature>()).Where(creature => creature != null && creature.CanSell))
                    {
                        totalValue += creature.GoldValue;
                        Destroy(creature.gameObject);
                    }
                }

                if (totalValue > 0)
                    GameData.Gold += totalValue;
            }
        }
    }
}
