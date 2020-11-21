using Dungeon.Scripts;
using UnityEngine;

namespace Dungeon.Objects
{
    public abstract class PlaceableObject : MonoBehaviour, ISellable
    {
        public virtual Vector2Int GridPosition { get; set; }
        public abstract bool CanSell { get; }
        public abstract int GoldValue { get; }

        public virtual void Destroy()
        {
            ObjectList.placedObjects.Remove(GridPosition);
            Destroy(gameObject);
        }
    }
}
