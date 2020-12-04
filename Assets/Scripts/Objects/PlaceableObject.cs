using Dungeon.Scripts;
using UnityEngine;

namespace Dungeon.Objects
{
    public abstract class PlaceableObject : MonoBehaviour, ISellable
    {
        [SerializeField]
        private bool setupOnGameStartup = false;
        [SerializeField]
        protected Vector2Int gridPosition;
        public virtual Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }
        public abstract bool CanSell { get; }
        public abstract int GoldValue { get; }

        public virtual void Start()
        {
            if (setupOnGameStartup)
                ObjectList.placedObjects.Add(gridPosition, this);
        }

        public virtual void Destroy()
        {
            ObjectList.placedObjects.Remove(GridPosition);
            Destroy(gameObject);
        }
    }
}
