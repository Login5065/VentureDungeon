using DG.Tweening;
using Dungeon.Audio;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public Texture2D cursorNeutral;

    void Start()
    {
        DOTween.SetTweensCapacity(1000, 100);
        CursorManager.SetCursor("White");
        MusicManager.Play("Night");
    }
}

public static class CursorManager
{
    public static Dictionary<string, CursorItem> cursors = new Dictionary<string, CursorItem>()
    {
        {"White", new CursorItem("CursorWhite")},
        {"Yellow", new CursorItem("CursorYellow")},
        {"Red", new CursorItem("CursorRed")},
        {"Purple", new CursorItem("CursorPurple")},
        {"Green", new CursorItem("CursorGreen")},
        {"Blue", new CursorItem("CursorBlue")},
        {"Architect", new CursorItem("CursorArchitect", false)},
        {"Attack", new CursorItem("CursorAttack", false)},
        {"Hammer", new CursorItem("CursorHammer", false)},
        {"Help", new CursorItem("CursorHelp", false)},
        {"Mine", new CursorItem("CursorMine", false)},
        {"Object", new CursorItem("CursorObject", false)},
        {"Target", new CursorItem("CursorTarget", false)},
        {"Cancel", new CursorItem("CursorCancel", false)},
    };
    public static void SetCursor(string cursor)
    {
        if (cursors.TryGetValue(cursor, out var val)) 
            Cursor.SetCursor(val.cursorTexture, val.hotspot, CursorMode.ForceSoftware);
    }
}

public class CursorItem
{
    public Texture2D cursorTexture;
    bool large = false;
    public Vector2 hotspot;

    public CursorItem(string texture, bool large = true)
    {
        cursorTexture = Resources.Load(texture) as Texture2D;
        this.large = large;
        if (large)
        {
            hotspot = new Vector2(10, 5);
        }
        else
        {
            hotspot = new Vector2(6, 3);
        }
    }
}