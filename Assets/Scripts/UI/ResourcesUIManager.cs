using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.UI
{
    public class ResourcesUIManager : MonoBehaviour
    {
        public static Text GoldText;
        public static Text FameText;
        public static Text ThreatText;
        // Start is called before the first frame update
        void Start()
        {
            GoldText = this.gameObject.transform.Find("Resources").Find("GoldAmount").GetComponent<Text>();
            FameText = this.gameObject.transform.Find("Resources").Find("FameAmount").GetComponent<Text>();
            ThreatText = this.gameObject.transform.Find("Resources").Find("ThreatAmount").GetComponent<Text>();
        }
    }

}
