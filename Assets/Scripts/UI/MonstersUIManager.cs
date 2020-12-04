using System.Collections.Generic;
using Dungeon.Pathfinding;
using Dungeon.Variables;
using Dungeon.Creatures;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class MonstersUIManager : MonoBehaviour
    {
        public Tilemap tilemap;
        private readonly List<Button> MonsterButtons = new List<Button>();
        public GameObject monsterSelected;
        public GameObject monsterSelectedGhost;
        public GameObject monsterGhost;
        private float checkDelay = 0;
        private int currentCost = -1;

        private void Start()
        {
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonSwordSkeleton").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonBowSkeleton").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonAxeSkeleton").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonDevSwordHero").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonDevBowHero").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonDevSpearHero").GetComponent<Button>());
            MonsterButtons.Add(Statics.LeftMenuMonsters.transform.Find("ButtonDevDragon").GetComponent<Button>());

            for (int i = 0; i < MonsterButtons.Count; i++)
            {
                var x = i;
                var button = MonsterButtons[x];
                button.onClick.AddListener(() => MonsterArchitectCallback(x));
            }
        }

        private void Update()
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.Monster)
            {
                if (monsterSelected != null)
                {
                    var click = Input.GetMouseButtonDown(0);
                    if ((click || checkDelay <= 0) && !UI.MouseInputUIBlocker.BlockedByUI)
                    {
                        HandlePlacement(click);
                        checkDelay = 0.05f;
                    }
                    else checkDelay -= Time.unscaledDeltaTime;
                }
                else
                {
                    Statics.UIManager.mode = (int)UIManager.UIModes.None;
                }
            }
            else
            {
                monsterSelected = null;
                if (monsterGhost != null)
                {
                    Object.Destroy(monsterGhost);
                    monsterGhost = null;
                }
            }
        }

        private void HandlePlacement(bool place)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var tilePos = tilemap.WorldToCell(mousePos);

            TilemapPathfinder.GetClosesValidPointBelow(tilemap, ref tilePos);
            if (TilemapPathfinder.CanStandOn(tilemap, (Vector2Int)tilePos) && TilemapPathfinder.CanFit(tilemap, (Vector2Int)tilePos, 3))
            {
                if (GameData.Gold >= currentCost || currentCost <= 0)
                {
                    HandleGhost(tilePos, true);
                    if (place)
                    {
                        Instantiate(monsterSelected, tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0), Quaternion.identity).GetComponent<Creature>().enabled = true;
                        if (currentCost != -1) GameData.Gold -= currentCost;
                    }
                }
                else HandleGhost(tilePos, false);
            }
            else HandleGhost(mousePos, false);
        }

        private void HandleGhost(Vector3Int tilePos, bool canPlace)
            => HandleGhost(Statics.TileMapFG.CellToWorld(tilePos) + new Vector3(0.5f, 0), canPlace);

        private void HandleGhost(Vector3 pos, bool canPlace)
        {
            if (monsterGhost == null)
            {
                monsterGhost = Instantiate(monsterSelectedGhost, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            }

            monsterGhost.transform.position = pos;
            if (canPlace)
                monsterGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 0.5f);
            else
                monsterGhost.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 0.5f);
        }

        private void MonsterArchitectCallback(int x)
        {
            Object.Destroy(monsterGhost);
            monsterSelected = null;
            monsterSelectedGhost = null;
            monsterGhost = null;
            currentCost = -1;
            switch (x)
            {
                case 0:
                    monsterSelected = Resources.Load<GameObject>("Creatures/SwordSkeleton");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/SwordSkeletonGhost");
                    break;
                case 1:
                    monsterSelected = Resources.Load<GameObject>("Creatures/BowSkeleton");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/BowSkeletonGhost");
                    break;
                case 2:
                    monsterSelected = Resources.Load<GameObject>("Creatures/AxeSkeleton");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/AxeSkeletonGhost");
                    break;
                case 3:
                    monsterSelected = Resources.Load<GameObject>("Creatures/SwordHero");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/SwordHeroGhost");
                    break;
                case 4:
                    monsterSelected = Resources.Load<GameObject>("Creatures/BowHero");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/BowHeroGhost");
                    break;
                case 5:
                    monsterSelected = Resources.Load<GameObject>("Creatures/SpearHero");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/SpearHeroGhost");
                    break;
                case 6:
                    monsterSelected = Resources.Load<GameObject>("Creatures/Dragon");
                    monsterSelectedGhost = Resources.Load<GameObject>("Creatures/DragonGhost");
                    break;
                default:
                    Statics.UIManager.mode = (int)UIManager.UIModes.None;
                    monsterSelected = null;
                    monsterSelectedGhost = null;
                    break;
            }

            if (monsterSelected != null && monsterSelected.TryGetComponent<Creature>(out var creature))
            {
                Statics.UIManager.mode = (int)UIManager.UIModes.Monster;

                var text = MonsterButtons[x].GetComponentInChildren<Text>();
                if (text != null && int.TryParse(text.text.Substring(0, text.text.Length - 3), out var cost))
                    currentCost = cost;
            }
        }
    }
}
