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
            Dictionary<Items, int> nearbyItemCounts = new Dictionary<Items, int>();

            foreach (Items item in nearbyItems)
            {
                if (nearbyItemCounts.ContainsKey(item))
                    nearbyItemCounts[item]++;
                else
                    nearbyItemCounts[item] = 1;
            }

            Dictionary<Items, int> recipeItemCounts = new Dictionary<Items, int>();

            foreach (Items inputItem in recipe.inputItems)
            {
                if (recipeItemCounts.ContainsKey(inputItem))
                    recipeItemCounts[inputItem]++;
                else
                    recipeItemCounts[inputItem] = 1;
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
                    Debug.Log($"Item mismatch: {entry.Key.name} expected {entry.Value}, found {nearbyItemCounts.GetValueOrDefault(entry.Key, 0)}");
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
            List<Items> itemsToRemove = new List<Items>(inputItems);

            for (int i = nearbyItems.Count - 1; i >= 0; i--)
            {
                Items currentItem = nearbyItems[i];

                for (int j = 0; j < itemsToRemove.Count; j++)
                {
                    if (currentItem == itemsToRemove[j])
                    {
                        if (nearbyItemObjects[i] != null)
                            Object.Destroy(nearbyItemObjects[i]);

                        itemsToRemove.RemoveAt(j);
                        nearbyItems.RemoveAt(i);
                        nearbyItemObjects.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
