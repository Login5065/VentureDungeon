using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon.Creatures;
using Dungeon.Variables;

namespace Dungeon.Objects
{
    public class Spikes : PlaceableObject
    {
        public int currentUses;
        public int maxUses;
        public int damage;
        public int secondsToDamage;

        public List<Creature> list;
        public List<Creature> toremove;

        public override bool CanSell => throw new System.NotImplementedException();

        public override int GoldValue => throw new System.NotImplementedException();

        public void Awake()
        {
            list = new List<Creature>();
            toremove = new List<Creature>();
            StartCoroutine(Hurt());
        }

        private IEnumerator Hurt()
        {
            while (true)
            {
                foreach (Creature c in list)
                {
                    if (c == null)
                    {
                        toremove.Add(c);
                    }
                    else if (c.Health > 0)
                    {
                        c.Health -= damage;
                        currentUses--;
                    }
                }
                foreach (Creature c in toremove)
                {
                    list.Remove(c);
                }
                toremove.Clear();
                if (currentUses <= 0)
                {
                    ObjectManager.KillObject(this);
                    yield break;
                }
                yield return new WaitForSeconds(secondsToDamage);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.shouldBeSeen && creature.allegiance == true) // true - bohater, false - potwor
            {
                //creature.Health -= damage;
                list.Add(creature);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.shouldBeSeen && creature.allegiance == true) // true - bohater, false - potwor
            {
                list.Remove(creature);
            }
        }
    }
}