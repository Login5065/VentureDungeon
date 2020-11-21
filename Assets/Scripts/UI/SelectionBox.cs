using UnityEngine;
using Dungeon.Variables;
using System;

namespace Dungeon.UI
{
    public class SelectionBox : MonoBehaviour
    {
        private bool isSelecting = false;
        private static Texture2D whiteTexture;
        private Vector3Int pos1;
        private Vector3Int pos2;

        public event Action<RectInt> OnSelectionFinished;

        private void Start()
        {
            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            // Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        void Update()
        {
            // If we press the left mouse button, save mouse location and begin selection
            if (Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
                pos1 = Statics.TileMapFG.WorldToCell(Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            }
            // If we let go of the left mouse button, end selection
            if (Input.GetMouseButtonUp(0))
            {
                isSelecting = false;
                var min = Vector2Int.Min((Vector2Int)pos1, (Vector2Int)pos2);
                var max = Vector2Int.Max((Vector2Int)pos1, (Vector2Int)pos2);
                var bounds = new RectInt(min, max - min);
                OnSelectionFinished(bounds);
            }
        }

        void OnGUI()
        {
            if (isSelecting)
            {
                // Create a rect from both mouse positions
                pos2 = Statics.TileMapFG.WorldToCell(Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                if (pos1 != pos2)
                {
                    var rect = GetScreenRect(Camera.main.WorldToScreenPoint(Statics.TileMapFG.CellToWorld(pos1)), Camera.main.WorldToScreenPoint(Statics.TileMapFG.CellToWorld(pos2)));
                    DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                    DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
                }
            }
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }
}
