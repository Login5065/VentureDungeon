using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Variables
{
    public static class Register
    {
        public enum MonsterTypes
        {
            SwordSkeleton,
            BowSkeleton,
            AxeSkeleton,
            SwordHero,
            BowHero,
            SpearHero,
            Dragon
        }
        public static List<string> MonsterNames = new List<string>()
        {
            "Sword Skeleton",
            "Bow Skeleton",
            "Axe Skeleton",
            "Sword Hero",
            "Bow Hero",
            "Spear Hero",
            "Black Dragon"
        };
        public enum TileTypes
        {
            Grass,
            Dirt,
            Stone,
            Rock,
            Glass,
            StoneBrick,
            BlackTile,
            BlueTile,
            BrownTile
        }
        public enum ObjectTypes
        {
            Treasure,
            Spikes,
            Vines,
            Lava,
            Mine,
            Torch,
            Entry
        }
    }

    public class IntObjectRegister<TValue> : ObjectRegister<int, TValue>
    {
        protected int id = 0;

        public int AddObject(TValue value)
        {
            objects.Add(++id, value);
            return id;
        }
    }

    public class ObjectRegister<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public Dictionary<TKey, TValue> objects = new Dictionary<TKey, TValue>();

        public ICollection<TKey> Keys => objects.Keys;
        public ICollection<TValue> Values => objects.Values;
        public int Count => objects.Count;
        public bool IsReadOnly => false;

        public TValue this[TKey key] { get => objects[key]; set => objects[key] = value; }


        public virtual bool AddObject(TKey key, TValue value)
        {
            if (objects.ContainsKey(key))
                return false;
            objects.Add(key, value);
            return true;
        }

        public virtual bool RemoveObject(TValue o)
        {
            TKey key = objects.FirstOrDefault(x => x.Value.Equals(o)).Key;
            return objects.Remove(key);
        }

        public virtual bool ContainsKey(TKey key) => objects.ContainsKey(key);

        public virtual bool ContainsValue(TValue value) => objects.ContainsValue(value);

        public virtual void Add(TKey key, TValue value) => objects.Add(key, value);

        public virtual bool Remove(TKey key) => objects.Remove(key);

        public virtual bool TryGetValue(TKey key, out TValue value) => objects.TryGetValue(key, out value);

        public virtual void Add(KeyValuePair<TKey, TValue> item) => objects.Add(item.Key, item.Value);

        public virtual void Clear() => objects.Clear();

        public virtual bool Contains(KeyValuePair<TKey, TValue> item) => objects.Contains(item);

        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (objects[item.Key].Equals(item.Value))
                return objects.Remove(item.Key);
            return false;
        }

        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => objects.GetEnumerator();
    }
}
