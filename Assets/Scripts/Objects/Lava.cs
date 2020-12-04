using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon.Creatures;
namespace Dungeon.Objects
{
    public class Lava : PlaceableObject
    {
        public int currentUses;
        public int maxUses;
        public int damage;
        public int secondsToDamage;
        public bool isActive;

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
                if (!isActive)
                {
                    yield return new WaitForSeconds(secondsToDamage);
                    isActive = true;
                }
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
                        isActive = false;
                    }
                }
                foreach (Creature c in toremove)
                {
                    list.Remove(c);
                }
                toremove.Clear();
                if (currentUses <= 0)
                {

                    Destroy(this.gameObject);
                    yield break;
                }
                yield return new WaitForSeconds(1);
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