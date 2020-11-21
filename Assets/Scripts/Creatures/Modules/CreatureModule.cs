using System;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class CreatureModule : MonoBehaviour
    {
        public Creature owner;
        public int priority = 0;
        public int chance = 100;
        public bool tried;
        public virtual bool Requirement()
        {
            return false;
        }
        public virtual void Update() { }
    }
}