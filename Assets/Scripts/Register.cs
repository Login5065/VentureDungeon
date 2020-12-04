using System.Collections.Generic;

namespace Dungeon.Variables
{
    public static class Register
    {
        public enum MonsterTypes
        {
            SwordSkeleton,
            BowSkeleton,
            AxeSkeleton,
            SwordHero,
            BowHero,
            SpearHero,
            Dragon
        }
        public static List<string> MonsterNames = new List<string>()
        {
            "Sword Skeleton",
            "Bow Skeleton",
            "Axe Skeleton",
            "Sword Hero",
            "Bow Hero",
            "Spear Hero",
            "Black Dragon"
        };
        public enum TileTypes
        {
            Grass,
            Dirt,
            Stone,
            Rock,
            Glass,
            StoneBrick,
            BlackTile,
            BlueTile,
            BrownTile
        }
        public enum ObjectTypes
        {
            Treasure,
            Spikes,
            Vines,
            Lava,
            Mine,
            Torch,
            //Entry
        }
    }
}
