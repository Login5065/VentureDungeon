using Dungeon.Variables;
using Dungeon.Graphics;
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
            owner.ChangeAnimationState("Die");
            owner.shouldBeSeen = false;
            owner.StartCoroutine(Die());
            return true;
        }

        IEnumerator Die()
        {
            var shader = owner.gameObject.AddComponent<DissolveEffect>();
            if (owner.value != 0) GameData.Gold += owner.value;
            if (owner.type != 0) GameData.Fame += 1;
            if (owner.type == 0)
            {
                GameData.Fame -= 1;
                GameData.Threat += 10;
            }
            yield return new WaitForSeconds(8);
            shader.StartDissolve(0.5f);
            yield return new WaitForSeconds(2);
            Destroy(owner.gameObject);
            yield break;
        }
    }
}