using DG.Tweening;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public Texture2D cursorNeutral;

    void Start()
    {
        DOTween.SetTweensCapacity(1000, 100);
        //Cursor.SetCursor(cursorNeutral, Vector2.zero, CursorMode.ForceSoftware);
    }
}
