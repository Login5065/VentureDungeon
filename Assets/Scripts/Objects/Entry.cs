using Dungeon.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Objects
{
    public class Entry : PlaceableObject
    {
        public override bool CanSell => false;
        public override int GoldValue => 0;

        public HashSet<Entry> Connections { get; protected set; } = null;

        public override void Start()
        {
            base.Start();
            ClearAndSetupConnections();
        }

        protected void ClearAndSetupConnections()
        {
            if (Connections != null)
                return;

            var connections = ObjectManager.register.Values.OfType<Entry>().Where(x => x.GridPosition.x == GridPosition.x);

            if (!connections.Contains(this))
                throw new Exception($"The list of placed objects does not contain this object on x=({GridPosition.x})!");
            if (connections.Count() == 1)
            {
                Connections = new HashSet<Entry>
                {
                    this,
                };
                return;
            }

            var valuesWithLists = connections.Where(x => x.Connections != null);

            HashSet<Entry> entryList;

            if (valuesWithLists.Count() > 0)
                entryList = valuesWithLists.First().Connections;
            else
                entryList = new HashSet<Entry>();

            foreach (var connection in connections)
            {
                connection.Connections = entryList;
                connection.Connections.Add(connection);
            }
        }

        public void RemoveFromConnections()
        {
            if (Connections != null)
                Connections.Remove(this);
        }
    }
}