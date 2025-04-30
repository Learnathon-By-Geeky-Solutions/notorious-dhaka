using UnityEngine;
using UnityEngine.UI;
using Scriptables;
using Manager;

public class EquipmentUIManager : MonoBehaviour
{
    [Header("References")]
    public EquipmentHolder equipmentHolder;
    public Inventory inventory;
    public Button unequipButton;

    [Header("UI Elements")]
    public GameObject equipmentPanel;
    public Image equippedIconSlot;

    private Items currentlyEquippedItem;

    private void Start()
    {
        if (unequipButton != null)
            unequipButton.onClick.AddListener(UnequipItem);

        if (equipmentPanel != null)
            equipmentPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleEquipmentPanel();
        }
    }

    private void ToggleEquipmentPanel()
    {
        if (equipmentPanel != null)
        {
            equipmentPanel.SetActive(!equipmentPanel.activeSelf);
        }
    }

    public void EquipItem(Items item)
    {
        if (item == null || item.prefab == null || item.itemType != ItemType.Equippable)
        {
            Debug.LogWarning("Invalid item to equip.");
            return;
        }

        inventory.RemoveItem(item, 1, false);
        equipmentHolder.Equip(item.prefab);
        currentlyEquippedItem = item;
        UpdateEquipmentUI(item);
    }

    public void UnequipItem()
    {
        if (equipmentHolder.equippedObject == null)
        {
            Debug.Log("Nothing to unequip.");
            return;
        }

        equipmentHolder.Unequip();

        if (currentlyEquippedItem != null)
        {
            inventory.AddItem(currentlyEquippedItem);
        }

        currentlyEquippedItem = null;
        UpdateEquipmentUI(null);
    }

    private void UpdateEquipmentUI(Items item)
    {
        if (equippedIconSlot == null) return;

        if (item != null)
        {
            equippedIconSlot.sprite = item.icon;
            equippedIconSlot.color = Color.white;
        }
        else
        {
            equippedIconSlot.sprite = null;
            equippedIconSlot.color = new Color(1, 1, 1, 0);
        }
    }

    // ✅ Used by GameManager to persist equipped item
    public Items GetEquippedItem()
    {
        return currentlyEquippedItem;
    }
}
