using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemZone;
using Manager;
using Scriptables;

namespace Crafting
{
    public class CraftingPlate : MonoBehaviour
    {
        public CraftingManager craftingManager;
        private readonly List<Items> nearbyItems = new();

        private void OnTriggerEnter(Collider other)
        {
            Details itemObject = other.GetComponent<Details>();
            if (itemObject != null && itemObject.item != null)
            {
                nearbyItems.Add(itemObject.item);
                Debug.Log($"Item added to crafting plate: {itemObject.item.name}");
            }
            else
            {
                Debug.LogWarning("Item is missing or not assigned on the colliding object.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Details itemObject = other.GetComponent<Details>();
            if (itemObject != null && itemObject.item != null)
            {
                nearbyItems.Remove(itemObject.item);
                Debug.Log($"Item removed from crafting plate: {itemObject.item.name}");
            }
            else
            {
                Debug.LogWarning("Item is missing or not assigned on the colliding object.");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                // Check if craftingManager is assigned before calling
                if (craftingManager != null)
                {
                    craftingManager.TryCraft(nearbyItems);
                    Debug.Log("Key Pressed");
                }
                else
                {
                    Debug.LogError("Crafting manager is not assigned.");
                }
            }
        }
    }
}
