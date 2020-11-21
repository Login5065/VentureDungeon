using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.UI;
using Dungeon.Variables;
using Dungeon.Creatures;

public class CreatureSelector : MonoBehaviour
{
    public static GameObject SelectedUI;
    private readonly List<GameObject> MonsterIcons = new List<GameObject>();
    private List<string> monsterNames = new List<string>()
    {
        "Hero",
        "Mace Skeleton",
        "Bow Skeleton",
        "Spear Skeleton"
    };
    // Start is called before the first frame update
    void Start()
    {
        SelectedUI = this.gameObject.transform.Find("Selected").gameObject;
        MonsterIcons.Add(SelectedUI.transform.Find("Image Wrap").Find("Hero").gameObject);
        MonsterIcons.Add(SelectedUI.transform.Find("Image Wrap").Find("MaceSkeleton").gameObject);
        MonsterIcons.Add(SelectedUI.transform.Find("Image Wrap").Find("BowSkeleton").gameObject);
        MonsterIcons.Add(SelectedUI.transform.Find("Image Wrap").Find("SpearSkeleton").gameObject);
        SelectedUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Statics.UIManager.SelectedCreature != null)
        {
            if (Statics.UIManager.SelectedCreature.maxHealth > 0)
            {
                SelectedUI.transform.Find("Bars").transform.Find("Healthbar").GetComponent<Image>().fillAmount = Statics.UIManager.SelectedCreature.health / Statics.UIManager.SelectedCreature.maxHealth;
            }
            else
            {
                SelectedUI.transform.Find("Bars").transform.Find("Healthbar").GetComponent<Image>().fillAmount = 0;
            }
            if (Statics.UIManager.SelectedCreature.maxResource > 0)
            {
                SelectedUI.transform.Find("Bars").transform.Find("Resourcebar").GetComponent<Image>().fillAmount = Statics.UIManager.SelectedCreature.resource / Statics.UIManager.SelectedCreature.maxResource;
            }
            else
            {
                SelectedUI.transform.Find("Bars").transform.Find("Resourcebar").GetComponent<Image>().fillAmount = 0;
            }
            if (Statics.UIManager.SelectedCreature.resourceType == 1)
            {
                SelectedUI.transform.Find("Bars").transform.Find("Resourcebar").GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                SelectedUI.transform.Find("Bars").transform.Find("Resourcebar").GetComponent<Image>().color = Color.white;
            }
        }
        if (Input.GetMouseButtonDown(0) && !MouseInputUIBlocker.BlockedByUI)
        {
            if (Statics.UIManager.mode == (int)UIManager.UIModes.None || Statics.UIManager.mode == (int)UIManager.UIModes.Move)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                RaycastHit2D hit = new RaycastHit2D();
                bool isHit = false;
                foreach(RaycastHit2D rhit in hits)
                {
                    if (rhit.collider != null && rhit.collider.isTrigger == false && rhit.collider.gameObject.GetComponent<Creature>() != null)
                    {
                        hit = rhit;
                        isHit = true;
                        break;
                    }
                }
                if(isHit)
                {
                    if (Statics.UIManager.SelectedCreature != null)
                    {
                        Statics.UIManager.SelectedCreature.selection.SetActive(false);
                        Statics.UIManager.SelectedCreature = null;
                    }
                    Statics.UIManager.SelectedCreature = hit.collider.gameObject.GetComponent<Creature>();
                    Statics.UIManager.SelectedCreature.selection.SetActive(true);
                    Statics.UIManager.mode = (int)UIManager.UIModes.Move;
                    SelectedUI.SetActive(true);
                    UIManager.SelectedName.text = monsterNames[Statics.UIManager.SelectedCreature.type];
                    foreach (GameObject icon in MonsterIcons)
                    {
                        icon.SetActive(false);
                    }
                    MonsterIcons[Statics.UIManager.SelectedCreature.type].SetActive(true);

                }
                else if (Statics.UIManager.mode == (int)UIManager.UIModes.Move)
                {
                    if (Statics.UIManager.SelectedCreature != null)
                    {
                        Statics.UIManager.SelectedCreature.selection.SetActive(false);
                        Statics.UIManager.SelectedCreature = null;
                    }
                    Statics.UIManager.mode = (int)UIManager.UIModes.None;
                    UIManager.SelectedName.text = "-";
                    SelectedUI.SetActive(false);
                }
            }
        }
    }
}
