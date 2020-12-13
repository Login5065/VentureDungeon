using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeon.UI;
using Dungeon.Variables;
using Dungeon.Creatures;
using System;

public class CreatureSelector : MonoBehaviour
{
    public static GameObject SelectedUI;
    public static Text SelectedName;
    private readonly List<GameObject> MonsterIcons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SelectedUI = Instantiate(Resources.Load<GameObject>("UI/Selected"), Statics.UI.transform, false);
        foreach (var item in Enum.GetNames(typeof(Register.MonsterTypes)))
        {
            MonsterIcons.Add(SelectedUI.transform.Find("Image Wrap").Find(item).gameObject);
        }
        SelectedUI.SetActive(false);
        SelectedName = SelectedUI.transform.Find("Name").GetComponent<Text>();
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
                        Statics.UIManager.SelectedCreature.material.AddOperation(0.6f, "_OutlineAlpha", 1.0f, 0.0f);
                        Statics.UIManager.SelectedCreature = null;
                    }
                    Statics.UIManager.SelectedCreature = hit.collider.gameObject.GetComponent<Creature>();
                    if (Statics.UIManager.SelectedCreature.allegiance) CursorManager.SetCursor("Green");
                    else CursorManager.SetCursor("Red");
                    Statics.UIManager.SelectedCreature.material.AddOperation(0.0f, "_OutlineAlpha", 1.0f, 0.6f);
                    Statics.UIManager.mode = (int)UIManager.UIModes.Move;
                    SelectedUI.SetActive(true);
                    SelectedName.text = Register.MonsterNames[Statics.UIManager.SelectedCreature.type];
                    foreach (GameObject icon in MonsterIcons)
                    {
                        icon.SetActive(false);
                    }
                    MonsterIcons[Statics.UIManager.SelectedCreature.type].SetActive(true);

                }
                else if (Statics.UIManager.mode == (int)UIManager.UIModes.Move)
                {
                    CursorManager.SetCursor("White");
                    if (Statics.UIManager.SelectedCreature != null)
                    {
                        Statics.UIManager.SelectedCreature.material.AddOperation(0.6f, "_OutlineAlpha", 1.0f, 0.0f);
                        Statics.UIManager.SelectedCreature = null;
                    }
                    Statics.UIManager.mode = (int)UIManager.UIModes.None;
                    SelectedName.text = "-";
                    SelectedUI.SetActive(false);
                }
            }
        }
    }
}
