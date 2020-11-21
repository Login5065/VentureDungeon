using UnityEngine;
using Dungeon.Projectiles;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Dungeon.Creatures
{
    public class RangedAttack : AttackModule
    {
        public float attack = 10;
        public GameObject projectile;
        public override bool Requirement()
        {
            return (owner.closestCreature != null && Vector2.Distance(owner.transform.position, owner.closestCreature.transform.position) < range);
        }
        public override bool Attack()
        {
            bool WouldCollideWith(Transform transform)
            {
                if (transform.TryGetComponent<Tilemap>(out _))
                    return true;
                //else if (transform.TryGetComponent<Creature>(out var _))
                //    return false;
                else return false;
            }

            var hits = Physics2D.LinecastAll(new Vector2(owner.transform.position.x, owner.transform.position.y + owner.height / 2), new Vector2(owner.closestCreature.transform.position.x, owner.closestCreature.transform.position.y + owner.closestCreature.height / 2));

            if (!hits.Any(x => WouldCollideWith(x.transform)))
            {
                owner.isAttacking = true;
                owner.animator.SetInteger("Anim", animator);
                if (owner.gameObject.transform.position.x > owner.closestCreature.transform.position.x)
                {
                    owner.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    owner.GetComponent<SpriteRenderer>().flipX = false;
                }
                return true;
            }
            else return false;
        }
        public override bool ExecuteBonk()
        {
            if (owner.closestCreature != null)
            {
                owner.isAttacking = true;
                var created = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + owner.height / 2), Quaternion.identity).GetComponent<Projectile>();
                created.maxDistance = range;
                created.allegiance = owner.allegiance;
                created.damage = attack;
                created.target = new Vector2(owner.closestCreature.transform.position.x, owner.closestCreature.transform.position.y + (owner.closestCreature.height / 2));
                //created.reTarget(closestCreature.transform.position.x, closestCreature.transform.position.y + closestCreature.height / 2);
                if (gameObject.transform.position.x > owner.closestCreature.transform.position.x)
                {
                    created.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    created.GetComponent<SpriteRenderer>().flipX = false;
                }
                return true;
            }
            else
            {
                owner.isAttacking = false;
                return false;
            }
        }
    }
}