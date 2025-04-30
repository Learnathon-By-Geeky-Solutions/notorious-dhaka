using UnityEngine;
using System.Collections.Generic;
using Scriptables;
using PlayerStatus;
using Manager;

public class GameManagerPersistent : MonoBehaviour
{
    public static GameManagerPersistent Instance;

    public List<InventorySlot> savedInventory = new();
    public Items savedEquippedItem;
    public float playerHealth;
    public float playerHunger;
    public float playerThirst;
    public float playerOxygen;
    public Vector3 playerPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManagerPersistent initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameData(Inventory inventory, EquipmentUIManager equipment, PlayerStatusManager player)
    {
        savedInventory.Clear();
        foreach (var slot in inventory.inventorySlots)
        {
            savedInventory.Add(new InventorySlot(slot.item, slot.quantity));
        }

        savedEquippedItem = equipment.GetEquippedItem();
        playerHealth = player.GetHealth();
        playerHunger = player.GetHunger();
        playerThirst = player.GetThirst();
        playerOxygen = player.GetOxygen();
        playerPosition = player.transform.position;
    }

    public void ApplyGameData(Inventory inventory, EquipmentUIManager equipment, PlayerStatusManager player, EquipmentHolder holder)
    {
        foreach (InventorySlot slot in savedInventory)
        {
            for (int i = 0; i < slot.quantity; i++)
                inventory.AddItem(slot.item);
        }

        if (savedEquippedItem != null)
            equipment.EquipItem(savedEquippedItem);

        player.SetAllStatus(playerHealth, playerHunger, playerThirst, playerOxygen);
        player.transform.position = playerPosition;
    }
}
