using Dungeon.Graphics;
using UnityEngine;

namespace Dungeon.Objects
{
    public class ObjectSpriteRandomizer : MonoBehaviour
    {
        [SerializeReference]
        private Sprite[] possibleSprites;
        ShaderEffects mat;

        private void Start() => RandomizeSpriteBasedOnPos();

        public bool RandomizeSpriteBasedOnPos()
        {
            if (possibleSprites.Length <= 0)
            {
                Debug.LogError($"{nameof(ObjectSpriteRandomizer)} does not have any sprites defined, cannot randomize.");
                return false;
            }

            var obj = GetComponent<PlaceableObject>();
            var sprite = GetComponent<SpriteRenderer>();

            if (obj != null && sprite != null)
            {
                var oldState = Random.state; // Backup old state

                Random.InitState(obj.GridPosition.GetHashCode()); // Set seed based on object grid position
                sprite.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
                mat = gameObject.AddComponent<ShaderEffects>();
                mat.AddOperation(0, "_GrassSpeed", 1, 8.0f);

                Random.state = oldState; // Restore old state
                return true;
            }
            else
            {
                Debug.LogError($"{nameof(ObjectSpriteRandomizer)} is missing {nameof(PlaceableObject)} or {nameof(SpriteRenderer)}, cannot randomize sprite.");
                return false;
            }
        }

        public bool RandomizeSprite()
        {
            if (possibleSprites.Length <= 0)
            {
                Debug.LogError($"{nameof(ObjectSpriteRandomizer)} does not have any sprites defined, cannot randomize.");
                return false;
            }

            var obj = GetComponent<PlaceableObject>();
            var sprite = GetComponent<SpriteRenderer>();

            if (obj != null && sprite != null)
            {
                sprite.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
                return true;
            }
            else
            {
                Debug.LogError($"{nameof(ObjectSpriteRandomizer)} is missing {nameof(PlaceableObject)} or {nameof(SpriteRenderer)}, cannot randomize sprite.");
                return false;
            }
        }
    }
}
