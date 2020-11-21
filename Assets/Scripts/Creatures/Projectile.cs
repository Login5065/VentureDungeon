using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Dungeon.Creatures;

namespace Dungeon.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public HashSet<Collider2D> collided;
        public Vector2 target;
        public float speed = 15f;
        // Max distance (in tiles) before the projectile is destroyed
        public float maxDistance = 10f;
        // Current distance travelled, set to (any) infinity to destroy on next Update() call
        private float distanceTravelled = 0f;
        public float damage;

        // If true won't be destroyed by hitting enemies
        public bool piercesEnemies = false;
        // If true won't be destroyed by hitting foreground/collidable tiles
        public bool piercesGround = false;

        // TODO change to creature
        private HashSet<Creature> piercedEnemies;

        // Should hit everything with different type
        public bool allegiance = false;



        private void Start()
        {
            // Verify rigidbody and/or collider?
            //if (!TryGetComponent<Rigidbody2D>(out var rigidBody) || rigidBody.bodyType != RigidbodyType2D.Kinematic || !TryGetComponent<Collider2D>(out _))
            //    throw new Exception("Collider and rigid body must be present and have kinematic body type");

            if (piercesEnemies)
                piercedEnemies = new HashSet<Creature>();

        }

        private void Update()
        {
            if (float.IsInfinity(distanceTravelled))
            {
                Destroy(gameObject);
                return;
            }

            var distance = speed * Time.deltaTime;
            if (distanceTravelled + distance >= maxDistance)
            {
                distance = maxDistance - distanceTravelled;
                distanceTravelled = float.PositiveInfinity;
            }
            else
                distanceTravelled += distance;

            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, distance);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Tilemap>(out _))
            {
                if (!piercesGround)
                    Destroy(gameObject);
            }
            else if (collision.TryGetComponent<Creature>(out var creature) && collision is BoxCollider2D && creature.allegiance != allegiance)
            {
                void Damage()
                {
                    creature.Health -= damage;
                }

                if (!piercesEnemies)
                {
                    Damage();
                    Destroy(gameObject);
                    return;
                }

                if (piercedEnemies.Contains(creature))
                    return;
                piercedEnemies.Add(creature);
                Damage();
            }
        }
    }
}
