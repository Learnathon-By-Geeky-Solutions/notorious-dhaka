using UnityEngine;
using UnityEngine.UI;
using Scriptables;
using Manager;

public class CraftingSlotUI : MonoBehaviour
{
    public int slotIndex;
    public Image iconDisplay;

    private Items assignedItem;
    private CraftingUIController craftingUI;
    private Inventory inventory;

    private void Start()
    {
        craftingUI = FindObjectOfType<CraftingUIController>();
        inventory = FindObjectOfType<Inventory>();

        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnSlotClicked);
    }

    public void AssignItem(Items item)
    {
        if (inventory == null || item == null)
            return;

        var slot = inventory.inventorySlots.Find(s => s.item == item);
        if (slot == null || slot.quantity < 1)
        {
            Debug.LogWarning("[CraftingSlotUI] Not enough item in inventory to assign.");
            return;
        }

        inventory.RemoveItem(item, 1, false);

        assignedItem = item;

        if (iconDisplay != null && item != null)
        {
            iconDisplay.sprite = item.icon;
            iconDisplay.color = Color.white;
        }

        craftingUI.AssignItemToSlot(slotIndex, item);
        craftingUI.ReevaluateCraftingOutput(); // Update preview after assignment
    }

    public void ClearSlot()
    {
        assignedItem = null;

        if (iconDisplay != null)
        {
            iconDisplay.sprite = null;
            iconDisplay.color = new Color(1, 1, 1, 0);
        }

        // Tell CraftingUI to clear the reference and re-check recipe
        craftingUI.AssignItemToSlot(slotIndex, null);
        craftingUI.ReevaluateCraftingOutput();
    }

    private void OnSlotClicked()
    {
        if (assignedItem != null && inventory != null)
        {
            inventory.AddItem(assignedItem);
            ClearSlot();
        }
    }

    public bool IsEmpty() => assignedItem == null;

    public Items GetItem() => assignedItem;
}
