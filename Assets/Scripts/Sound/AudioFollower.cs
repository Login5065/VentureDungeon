using UnityEngine;

namespace Dungeon.Audio
{
    public class AudioFollower : MonoBehaviour
    {
        private DragCamera2D dragCamera;

        private void Awake() => dragCamera = GetComponentInParent<DragCamera2D>();

        private void Update() => transform.position = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.orthographicSize - dragCamera.minZoom);
    }
}