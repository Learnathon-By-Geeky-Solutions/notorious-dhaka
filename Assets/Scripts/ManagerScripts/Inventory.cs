using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class Inventory : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots = new();
        public GameObject inventoryPanel;
        public GameObject itemUIPrefab;

        public void AddItem(Items item)
        {
            InventorySlot existingSlot = inventorySlots.Find(slot => slot.item == item);
            if (existingSlot != null)
            {
                existingSlot.quantity++;
                UpdateUI(existingSlot);
            }
            else
            {
                InventorySlot newSlot = new InventorySlot(item, 1);
                inventorySlots.Add(newSlot);

                GameObject newItemUI = Instantiate(itemUIPrefab, inventoryPanel.transform);
                newSlot.uiElement = newItemUI;

                Button removeButton = newItemUI.GetComponentInChildren<Button>();
                if (removeButton != null)
                {
                    var capturedItem = item;
                    removeButton.onClick.AddListener(() => RemoveItem(capturedItem));
                }

                UpdateUI(newSlot);
            }
        }

        public void RemoveItem(Items item)
        {
            InventorySlot slot = inventorySlots.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity--;

                if (slot.quantity <= 0)
                {
                    inventorySlots.Remove(slot);
                    if (slot.uiElement != null)
                    {
                        Destroy(slot.uiElement);
                    }
                }
                else
                {
                    UpdateUI(slot);
                }

                SpawnItem(item);
            }
            else
            {
                Debug.LogWarning($"Item not found in inventory: {item.name}");
            }
        }

        private void SpawnItem(Items item)
        {
            Vector3 SpawnPosition = transform.position + new Vector3(3, 0, 3);
            if (item.prefab != null)
            {
                Instantiate(item.prefab, SpawnPosition, Quaternion.identity);
                Debug.Log($"Spawned item: {item.name}");
            }
            else
            {
                Debug.LogWarning($"Prefab missing for item: {item.name}");
            }
        }

        private void UpdateUI(InventorySlot slot)
        {
            if (slot.uiElement != null)
            {
                var itemText = slot.uiElement.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (itemText != null)
                {
                    itemText.text = $"{slot.item.name} x{slot.quantity}";
                }

                var itemImage = slot.uiElement.GetComponentInChildren<Image>();
                if (itemImage != null)
                {
                    itemImage.sprite = slot.item.icon;
                }
            }
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public Items item;
        public int quantity;
        public GameObject uiElement;

        public InventorySlot(Items item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
            this.uiElement = null;
        }
    }
}
