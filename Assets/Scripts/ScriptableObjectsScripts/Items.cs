using UnityEngine;

namespace Scriptables
{
    public enum ItemType
    {
        Equippable,
        NonEquippable,
        Placeable,
        Consumable // ✅ New item type added
    }

    [CreateAssetMenu(menuName = "Game/Item")]
    public class Items : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public GameObject prefab;
        [TextArea(3, 5)] public string description;

        public ItemType itemType;

        public float healAmount = 10f; // ✅ Added heal amount (only useful for Consumable)
    }
}
