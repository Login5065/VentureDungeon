using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.UI
{
    [Serializable]
    public class BaseUIElement
    {
        #region Unity inspector fields
        [SerializeReference]
        private GameObject spawnObject;
        [SerializeField]
        private string name = "Missing name";
        [SerializeField]
        private int cost;
        [SerializeField]
        private int data = int.MinValue;
        #endregion

        public GameObject SpawnObject { get => spawnObject; set => spawnObject = value; }
        public int Data { get => data; set => data = value; }
        public string Name { get => name; set => name = value ?? "Missing name"; }
        // cost == 0 - free
        // cost < 0 - debug
        public int Cost { get => Math.Max(0, cost); set => cost = value; }
        public bool IsDebug => cost < 0;
    }

    public class BaseUIElementHolder : MonoBehaviour
    {
        #region Base element and fast access properties
        public BaseUIElement element;
        private Text costText;
        private Text itemText;
        private Image itemIcon;

        public GameObject SpawnObject { get => element.SpawnObject; set => element.SpawnObject = value; }
        public int Data { get => element.Data; set => element.Data = value; }
        public string Name { get => element.Name; set => element.Name = value; }
        public int Cost { get => element.Cost; set => element.Cost = value; }
        public bool IsDebug => element.IsDebug;
        #endregion

        public void Start()
        {
            costText = gameObject.transform.Find("ShopPrice").GetComponent<Text>();
            itemText = gameObject.transform.Find("ShopItemName").GetComponent<Text>();
            itemIcon = gameObject.transform.Find("ShopIcon").GetComponent<Image>();
        }

        public void UpdateData()
        {
            if (IsDebug) { costText.text = "DEV"; }
            else { costText.text = Cost.ToString(); }
            itemText.text = Name;
        }
    }
}