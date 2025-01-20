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
        }

        private void OnTriggerExit(Collider other)
        {
            Details itemObject = other.GetComponent<Details>();
            if (itemObject != null && itemObject.item != null)
            {
                nearbyItems.Remove(itemObject.item);
                Debug.Log($"Item removed from crafting plate: {itemObject.item.name}");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                craftingManager.TryCraft(nearbyItems);
                Debug.Log("Key Pressed");
            }
        }
    }
}