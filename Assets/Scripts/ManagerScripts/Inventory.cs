using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptables;
using PlayerStatus;

namespace Manager
{
    public class Inventory : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots = new();
        public GameObject inventoryPanel;
        public GameObject itemUIPrefab;

        private CraftingUIController craftingUI;
        private UseItemManager useItemManager;
        private PlaceableSpawner placeableSpawner;
        private EquipmentUIManager equipmentUI;

        private void Start()
        {
            craftingUI = FindObjectOfType<CraftingUIController>();
            useItemManager = FindObjectOfType<UseItemManager>();
            placeableSpawner = FindObjectOfType<PlaceableSpawner>();
            equipmentUI = FindObjectOfType<EquipmentUIManager>();
        }

        public void AddItem(Items item)
        {
            if (item == null) return;

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

                var rightClickHandler = newItemUI.AddComponent<InventoryItemRightClickHandler>();
                rightClickHandler.Initialize(item);

                Button removeButton = newItemUI.GetComponentInChildren<Button>();
                if (removeButton != null)
                {
                    var capturedItem = item;

                    removeButton.onClick.AddListener(() =>
                    {
                        Debug.Log("[Inventory] Clicked item: " + capturedItem.name);

                        if (capturedItem == null || capturedItem.prefab == null)
                        {
                            Debug.LogWarning("[Inventory] Invalid item or missing prefab.");
                            return;
                        }

                        switch (capturedItem.itemType)
                        {
                            case ItemType.Equippable:
                                equipmentUI?.EquipItem(capturedItem);
                                break;

                            case ItemType.NonEquippable:
                                if (craftingUI != null && craftingUI.gameObject.activeSelf)
                                {
                                    foreach (var slot in FindObjectsOfType<CraftingSlotUI>())
                                    {
                                        if (slot.IsEmpty())
                                        {
                                            slot.AssignItem(capturedItem);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    useItemManager?.UseItem(capturedItem);
                                    RemoveItem(capturedItem, 1, false);
                                }
                                break;

                            case ItemType.Placeable:
                                if (craftingUI != null && craftingUI.gameObject.activeSelf)
                                {
                                    foreach (var slot in FindObjectsOfType<CraftingSlotUI>())
                                    {
                                        if (slot.IsEmpty())
                                        {
                                            slot.AssignItem(capturedItem);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    RemoveItem(capturedItem, 1, true);
                                    placeableSpawner?.StartPlacing(capturedItem);
                                }
                                break;

                            case ItemType.Consumable: // ✅ NEW: Consumable
                                var playerStatus = FindObjectOfType<PlayerStatusManager>();
                                if (playerStatus != null)
                                {
                                    playerStatus.HealPlayer(capturedItem.healAmount);
                                }
                                RemoveItem(capturedItem, 1, false);
                                Debug.Log("[Inventory] Consumed " + capturedItem.name + ", healed +" + capturedItem.healAmount + " health.");
                                break;
                        }
                    });
                }

                UpdateUI(newSlot);
            }
        }

        public void RemoveItem(Items item, int amount, bool spawnInWorld = false)
        {
            InventorySlot slot = inventorySlots.Find(s => s.item == item);
            if (slot != null)
            {
                slot.quantity -= amount;

                if (slot.quantity <= 0)
                {
                    inventorySlots.Remove(slot);
                    if (slot.uiElement != null)
                        Destroy(slot.uiElement);
                }
                else
                {
                    UpdateUI(slot);
                }

                if (spawnInWorld)
                    SpawnItem(item);
            }
            else
            {
                Debug.LogWarning("[Inventory] Item not found: " + item.name);
            }
        }

        private void SpawnItem(Items item)
        {
            Vector3 spawnPosition = transform.position + new Vector3(2f, 1f, 1.5f);

            if (item.prefab != null)
            {
                GameObject spawnedItem = Instantiate(item.prefab, spawnPosition, Quaternion.identity);

                Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                Debug.Log("[Inventory] Spawned item: " + item.name);
            }
            else
            {
                Debug.LogWarning("[Inventory] Prefab is missing for: " + item.name);
            }
        }

        private void UpdateUI(InventorySlot slot)
        {
            if (slot.uiElement != null)
            {
                var itemText = slot.uiElement.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (itemText != null)
                    itemText.text = $"{slot.item.name} x{slot.quantity}";

                var itemImage = slot.uiElement.GetComponentInChildren<Image>();
                if (itemImage != null)
                    itemImage.sprite = slot.item.icon;
            }
        }

        public Items GetItemByName(string name)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.item.itemName == name)
                    return slot.item;
            }
            return null;
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
