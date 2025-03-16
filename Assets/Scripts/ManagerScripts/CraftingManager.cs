using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting;
using Scriptables;

namespace Manager
{
    public class CraftingManager : MonoBehaviour
    {
        public List<CraftingRecipe> recipes;
        public Transform craftingPlate;

        public void TryCraft(List<Items> nearbyItems, List<GameObject> nearbyItemObjects)
        {
            if (recipes == null || recipes.Count == 0)
            {
                Debug.LogError("No recipes assigned to the crafting manager!");
                return;
            }

            foreach (CraftingRecipe recipe in recipes)
            {
                if (recipe.outputItem == null)
                {
                    Debug.LogError($"Output item is missing in the recipe for {recipe.name}");
                    continue;
                }

                Debug.Log($"Checking recipe: {recipe.outputItem.name}");

                if (IsRecipeMatch(recipe, nearbyItems))
                {
                    CraftItem(recipe);
                    RemoveItems(nearbyItems, nearbyItemObjects, recipe.inputItems);
                    return;
                }
            }

            Debug.Log("No matching recipe found.");
        }

        private bool IsRecipeMatch(CraftingRecipe recipe, List<Items> nearbyItems)
        {
            Dictionary<string, int> nearbyItemCounts = new Dictionary<string, int>();

            foreach (Items item in nearbyItems)
            {
                if (nearbyItemCounts.ContainsKey(item.name))
                    nearbyItemCounts[item.name]++;
                else
                    nearbyItemCounts[item.name] = 1;
            }

            Dictionary<string, int> recipeItemCounts = new Dictionary<string, int>();

            foreach (Items inputItem in recipe.inputItems)
            {
                if (recipeItemCounts.ContainsKey(inputItem.name))
                    recipeItemCounts[inputItem.name]++;
                else
                    recipeItemCounts[inputItem.name] = 1;
            }

            if (nearbyItemCounts.Count != recipeItemCounts.Count)
            {
                Debug.Log("Nearby items do not match the recipe exactly.");
                return false;
            }

            foreach (var entry in recipeItemCounts)
            {
                if (!nearbyItemCounts.ContainsKey(entry.Key) || nearbyItemCounts[entry.Key] != entry.Value)
                {
                    Debug.Log($"Item mismatch: {entry.Key} expected {entry.Value}, found {nearbyItemCounts.GetValueOrDefault(entry.Key, 0)}");
                    return false;
                }
            }

            return true;
        }

        private void CraftItem(CraftingRecipe recipe)
        {
            if (recipe.outputItem != null && recipe.outputItem.prefab != null)
            {
                if (craftingPlate != null)
                {
                    Instantiate(recipe.outputItem.prefab, craftingPlate.position + Vector3.up, Quaternion.identity);
                    Debug.Log($"Crafted: {recipe.outputItem.name}");
                }
                else
                {
                    Debug.LogError("Crafting plate is not assigned.");
                }
            }
            else
            {
                Debug.LogError("Output item or prefab is not assigned in the recipe.");
            }
        }

        private void RemoveItems(List<Items> nearbyItems, List<GameObject> nearbyItemObjects, List<Items> inputItems)
        {
            Dictionary<string, int> itemsToRemove = new Dictionary<string, int>();

            foreach (Items inputItem in inputItems)
            {
                if (itemsToRemove.ContainsKey(inputItem.name))
                    itemsToRemove[inputItem.name]++;
                else
                    itemsToRemove[inputItem.name] = 1;
            }

            for (int i = nearbyItems.Count - 1; i >= 0; i--)
            {
                string itemName = nearbyItems[i].name;
                if (itemsToRemove.ContainsKey(itemName) && itemsToRemove[itemName] > 0)
                {
                    // Destroy the actual GameObject in the scene
                    Destroy(nearbyItemObjects[i]);

                    itemsToRemove[itemName]--;
                    nearbyItems.RemoveAt(i);
                    nearbyItemObjects.RemoveAt(i); // Ensure the game object list stays in sync
                }
            }
        }
    }
}
