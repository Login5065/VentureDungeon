using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public GameObject ItemObject;

    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite GoldIdolSprite;
    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    private void Awake()
    {
        Instance = this;
    
    }

   

}
