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
            GoldText = gameObject.transform.Find("GoldAmount").GetComponent<Text>();
            FameText = gameObject.transform.Find("FameAmount").GetComponent<Text>();
            ThreatText = gameObject.transform.Find("ThreatAmount").GetComponent<Text>();
        }
    }

}
