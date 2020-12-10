using Dungeon.Scripts;
using Dungeon.Variables;
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
        public bool floating = false;

        public virtual void Start()
        {
            if (setupOnGameStartup)
                ObjectManager.register.Add(gridPosition, this);
        }
    }
}
