using UnityEngine;

namespace Code.Battle.Information
{
    public class Item : ScriptableObject
    {
        [Header("物品信息")]
        public Sprite itemIcon;
        public string itemName;
    }
}
