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

        // Store items and their respective game objects
        private readonly List<GameObject> nearbyItemObjects = new();
        private readonly List<Items> nearbyItems = new();

        private void OnTriggerEnter(Collider other)
        {
            Details itemObject = other.GetComponent<Details>();
            if (itemObject != null && itemObject.item != null)
            {
                nearbyItems.Add(itemObject.item);
                nearbyItemObjects.Add(other.gameObject); // Track the actual object
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
                int index = nearbyItems.IndexOf(itemObject.item);
                if (index >= 0)
                {
                    nearbyItems.RemoveAt(index);
                    nearbyItemObjects.RemoveAt(index); // Remove corresponding GameObject
                }
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
                if (craftingManager != null)
                {
                    craftingManager.TryCraft(nearbyItems, nearbyItemObjects); // Pass objects too
                    Debug.Log("Crafting Attempted");
                }
                else
                {
                    Debug.LogError("Crafting manager is not assigned.");
                }
            }
        }
    }
}
