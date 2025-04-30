using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptables;
using Manager;
using Crafting;

public class CraftingUIController : MonoBehaviour
{
    public GameObject craftingCanvas;
    public Button craftButton;
    public Image[] inputSlots; // Set size to 4 in Inspector
    public Image outputSlot;
    public Text messageText;

    public List<CraftingRecipe> allRecipes;
    private Items[] selectedItems = new Items[4];
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        craftingCanvas.SetActive(false);
        craftButton.onClick.AddListener(AttemptCraft);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (HasCraftingPlate())
                ToggleCraftingUI();
            else
                ShowMessage("Crafting plate required.");
        }
    }

    private bool HasCraftingPlate()
    {
        return inventory != null &&
               inventory.inventorySlots.Exists(slot => slot != null && slot.item != null && slot.item.itemName.ToLower().Contains("crafting plate"));
    }

    private void ToggleCraftingUI()
    {
        craftingCanvas.SetActive(!craftingCanvas.activeSelf);
    }

    public void AssignItemToSlot(int slotIndex, Items item)
    {
        if (item == null || slotIndex < 0 || slotIndex >= selectedItems.Length)
            return;

        selectedItems[slotIndex] = item;
        inputSlots[slotIndex].sprite = item.icon;
        inputSlots[slotIndex].color = Color.white;

        CheckRecipePreview();
    }

    private void CheckRecipePreview()
    {
        if (allRecipes == null || allRecipes.Count == 0)
        {
            outputSlot.sprite = null;
            outputSlot.color = new Color(1, 1, 1, 0);
            return;
        }

        foreach (var recipe in allRecipes)
        {
            if (recipe != null && recipe.outputItem != null && MatchRecipe(recipe))
            {
                outputSlot.sprite = recipe.outputItem.icon;
                outputSlot.color = Color.white;
                return;
            }
        }

        outputSlot.sprite = null;
        outputSlot.color = new Color(1, 1, 1, 0); // Transparent when no recipe matched
    }

    private void AttemptCraft()
    {
        if (inventory == null || allRecipes == null)
        {
            ShowMessage("Crafting setup incomplete.");
            return;
        }

        foreach (var recipe in allRecipes)
        {
            if (recipe != null && recipe.outputItem != null && MatchRecipe(recipe))
            {
                // ❌ NO MORE RemoveItem(input, 1) here
                // Because the items were already removed from inventory during AssignItem

                // ✅ Only give the output item
                inventory.AddItem(recipe.outputItem);
                ShowMessage($"Crafted: {recipe.outputItem.itemName}");

                ClearInputs();
                return;
            }
        }

        ShowMessage("No valid recipe.");
    }

    private bool MatchRecipe(CraftingRecipe recipe)
    {
        if (recipe == null || recipe.inputItems == null)
            return false;

        List<Items> inputs = new List<Items>();
        foreach (var item in selectedItems)
        {
            if (item != null)
                inputs.Add(item);
        }

        foreach (var required in recipe.inputItems)
        {
            if (required == null)
                continue;

            if (inputs.Contains(required))
                inputs.Remove(required);
            else
                return false;
        }

        return true;
    }

    private void ClearInputs()
    {
        for (int i = 0; i < selectedItems.Length; i++)
        {
            selectedItems[i] = null;
            inputSlots[i].sprite = null;
            inputSlots[i].color = new Color(1, 1, 1, 0);
        }

        foreach (var slot in FindObjectsOfType<CraftingSlotUI>())
        {
            slot.ClearSlot();
        }

        outputSlot.sprite = null;
        outputSlot.color = new Color(1, 1, 1, 0);
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    public void ReevaluateCraftingOutput()
    {
        CheckRecipePreview();
    }
}
