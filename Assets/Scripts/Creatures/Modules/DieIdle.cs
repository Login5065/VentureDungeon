using Dungeon.Variables;
using System.Collections;
using UnityEngine;

namespace Dungeon.Creatures
{
    public class DieIdle : IdleModule
    {
        public override bool Requirement()
        {
            return owner.health <= 0;
        }
        public override bool Idle()
        {
            owner.dying = true;
            owner.health = 0;
            owner.animator.SetInteger("Anim", -1);
            owner.shouldBeSeen = false;
            owner.StartCoroutine(Die());
            return true;
        }

        IEnumerator Die()
        {
            if (owner.value != 0) GameData.Gold += owner.value;
            if (owner.type != 0) GameData.Fame += 1;
            if (owner.type == 0)
            {
                GameData.Fame -= 1;
                GameData.Threat += 10;
            }
            yield return new WaitForSeconds(10);
            Destroy(this.gameObject);
            yield break;
        }
    }
}