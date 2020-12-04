using Dungeon.MapSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Editor
{
    class TilemapMenuItem
    {
        [MenuItem("Assets/Create/2D/Tiles/Extended Tile", priority = 98)]
        static void CreateExtendedTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<ExtendedRuleTile>(), "New Extended Tile.asset");
        }

        [MenuItem("Assets/Create/2D/Tiles/Extended Rule Tile", priority = 99)]
        static void CreateExtendedRuleTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<ExtendedRuleTile>(), "New Extended Rule Tile.asset");
        }

        [MenuItem("CONTEXT/Tile/To ExtendedTile")]
        static void RuleTileToExtendedTile(MenuCommand command)
        {
            var tile = command.context as Tile;

            if (tile is ExtendedTile)
                return;

            var newTile = ScriptableObject.CreateInstance<ExtendedTile>();
            newTile.hideFlags = tile.hideFlags;
            newTile.colliderType = tile.colliderType;
            newTile.color = tile.color;
            newTile.flags = tile.flags;
            newTile.sprite = tile.sprite;
            newTile.transform = tile.transform;
            newTile.name = tile.name;

            ProjectWindowUtil.CreateAsset(newTile, "Transformed Extended Tile.asset");
        }

        [MenuItem("CONTEXT/RuleTile/To ExtendedRuleTile")]
        static void RuleTileToExtendedRuleTile(MenuCommand command)
        {
            var tile = command.context as RuleTile;

            if (tile is ExtendedRuleTile)
                return;

            var newTile = ScriptableObject.CreateInstance<ExtendedRuleTile>();
            newTile.hideFlags = tile.hideFlags;
            newTile.m_DefaultColliderType = tile.m_DefaultColliderType;
            newTile.m_DefaultGameObject = tile.m_DefaultGameObject;
            newTile.m_DefaultSprite = tile.m_DefaultSprite;
            newTile.m_TilingRules = tile.m_TilingRules;
            newTile.name = tile.name;

            ProjectWindowUtil.CreateAsset(newTile, "Transformed Extended Tile.asset");
        }
    }
}
