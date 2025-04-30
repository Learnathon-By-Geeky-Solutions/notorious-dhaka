using UnityEngine;
using UnityEngine.EventSystems;
using Scriptables;
using PlayerStatus;
using Manager;

public class InventoryItemRightClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Items thisItem;
    private Inventory inventory;
    private PlayerStatusManager playerStatus;

    public void Initialize(Items item)
    {
        thisItem = item;
        inventory = FindObjectOfType<Inventory>();
        playerStatus = FindObjectOfType<PlayerStatusManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (thisItem == null || inventory == null || playerStatus == null)
                return;

            if (thisItem.itemType == ItemType.NonEquippable &&
                thisItem.prefab != null &&
                thisItem.prefab.CompareTag("mum")) // ✅ Tag-based check
            {
                inventory.RemoveItem(thisItem, 1, false);
                playerStatus.HealPlayer(10f);
                Debug.Log("[Inventory] Water consumed. +10 Health.");
            }
        }
    }
}
