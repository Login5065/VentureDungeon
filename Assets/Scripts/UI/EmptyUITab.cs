using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class EmptyUITab : BaseUITab
    {

        public override void SetInactive()
        {
            base.SetInactive();
        }

        protected override void OnItemUpdate(Vector3 mousePos, bool shouldTryPlace)
        {
            
        }

        protected override void OnItemSelected(Button button, BaseUIElementHolder item)
        {

        }
    }
}

