using UnityEngine;

namespace Dungeon.Objects
{
    public class Treasure : PlaceableObject
    {
        public int currentGold;
        public int maxGold;
        public Animator animator;
        private bool opened = false;
        public override bool CanSell => true;
        public override int GoldValue => currentGold;
        public void OpenAnimate()
        {
            if(!opened)
            {
                animator.Play("Open");
                opened = true;
            }
        }
        public override void Start()
        {
            base.Start();
            animator = gameObject.GetComponent<Animator>();
        }
    }
}
