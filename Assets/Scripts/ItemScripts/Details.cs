using UnityEngine;
using Manager;
using Scriptables;
using PlayerInteract;

namespace ItemZone
{
    public class Details : MonoBehaviour
    {
        public Items item;
        public Inventory storage;

        void Start()
        {
            storage = FindObjectOfType<Inventory>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var holder = collision.gameObject.GetComponentInChildren<EquipmentHolder>();

                if (holder != null && holder.equippedObject != null)
                {
                    string equippedName = holder.equippedObject.name.Replace("(Clone)", "").Trim();
                    if (equippedName == item.itemName)
                    {
                        Debug.Log("[Details] Already equipped. Ignoring pickup.");
                        return;
                    }
                }

                if (item != null && storage != null)
                {
                    switch (item.itemType)
                    {
                        case ItemType.Equippable:
                        case ItemType.NonEquippable:
                        case ItemType.Consumable: // ✅ New case: Consumable items go to Inventory
                            storage.AddItem(item);
                            PlayerItemInteraction.Instance?.PlayCollectSound();
                            Destroy(gameObject);
                            break;

                        case ItemType.Placeable:
                            FindObjectOfType<PlaceableSpawner>()?.StartPlacing(item);
                            PlayerItemInteraction.Instance?.PlayCollectSound();
                            Destroy(gameObject);
                            break;
                    }
                }
            }
        }
    }
}
