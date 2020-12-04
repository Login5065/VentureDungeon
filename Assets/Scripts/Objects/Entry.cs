using System;
using UnityEngine;

namespace Dungeon.Objects
{
    public class Entry : PlaceableObject
    {
        public override bool CanSell => false;
        public override int GoldValue => 0;

        [SerializeReference]
        protected GameObject connectedGameObject = null;
        public Entry Connection { get; protected set; }

        public Entry() { }

        public Entry(Entry connection)
        {
            if (connection == this)
                throw new ArgumentException("Can't connect an entry to itself", nameof(connection));
            else if (connection.GridPosition == GridPosition)
                throw new ArgumentException("Can't place both entries in the same spot", nameof(connection));

            Connection = connection;
            connection.Connection = this;
        }

        public void Awake()
        {
            if (Connection == null && connectedGameObject != null && connectedGameObject.TryGetComponent<Entry>(out var connected))
            {
                Connection = connected;
                connected.Connection = this;
            }
        }
    }
}
