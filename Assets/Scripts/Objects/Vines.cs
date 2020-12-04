using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon.Creatures;
namespace Dungeon.Objects
{
    public class Vines : PlaceableObject
    {
        public int damage;
        public int secondsToLetGo;
        public bool isActive;

        Creature victim;
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

        public void Update()
        {
            if (isActive)
            {
                victim.path = null;
            }
        }

        private IEnumerator Hurt()
        {
            while (true)
            {
                if (isActive)
                {
                    for (int i = 0; i < secondsToLetGo; i++)
                    {
                        victim.path = null;
                        yield return new WaitForSeconds(1);
                    }
                    isActive = false;
                }
                yield return new WaitForSeconds(1);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Creature>(out var creature) && other is BoxCollider2D && creature.shouldBeSeen && creature.allegiance == true) // true - bohater, false - potwor
            {
                if (!isActive)
                {
                    list.Add(creature);
                    victim = creature;
                    isActive = true;
                }
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