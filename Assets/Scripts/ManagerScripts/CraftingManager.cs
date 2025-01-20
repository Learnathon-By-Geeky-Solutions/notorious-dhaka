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

        public void TryCraft(List<Items> nearbyItems)
        {
            foreach (CraftingRecipe recipe in recipes)
            {
                Debug.Log($"Checking recipe: {recipe.outputItem.name}");

                if (IsRecipeMatch(recipe, nearbyItems))
                {
                    CraftItem(recipe);
                    RemoveItems(nearbyItems, recipe.inputItems);
                    return;
                }
            }

            Debug.Log("No matching recipe found.");
        }

        private bool IsRecipeMatch(CraftingRecipe recipe, List<Items> nearbyItems)
        {
            Dictionary<string, int> itemCounts = new Dictionary<string, int>();

            foreach (Items item in nearbyItems)
            {
                if (itemCounts.ContainsKey(item.name))
                    itemCounts[item.name]++;
                else
                    itemCounts[item.name] = 1;
            }

            foreach (Items inputItem in recipe.inputItems)
            {
                if (!itemCounts.ContainsKey(inputItem.name) || itemCounts[inputItem.name] <= 0)
                {
                    Debug.Log($"Item {inputItem.name} is missing or insufficient.");
                    return false;
                }

                itemCounts[inputItem.name]--;
            }

            return true;
        }

        private void CraftItem(CraftingRecipe recipe)
        {
            if (recipe.outputItem.prefab != null)
            {
                Instantiate(recipe.outputItem.prefab, craftingPlate.position + Vector3.up, Quaternion.identity);
                Debug.Log($"Crafted: {recipe.outputItem.name}");
            }
        }

        private void RemoveItems(List<Items> nearbyItems, List<Items> inputItems)
        {
            foreach (Items inputItem in inputItems)
            {
                for (int i = 0; i < nearbyItems.Count; i++)
                {
                    if (nearbyItems[i].name == inputItem.name)
                    {
                        nearbyItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
