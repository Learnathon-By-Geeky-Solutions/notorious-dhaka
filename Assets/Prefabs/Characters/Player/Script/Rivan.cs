using UnityEngine;
using Manager;     // To access CraftingManager
using Crafting;    // To access CraftingPlate

namespace Crafting
{
    public class Rivan : MonoBehaviour
    {
        public GameObject craftingPlatePrefab; // Assign your CraftingPlate prefab in the Inspector
        public float spawnDistance = 2f;       // Distance in front of Rivan where the plate will appear

        private GameObject currentCraftingPlate; // Reference to the spawned plate

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ToggleCraftingPlate();
            }
        }

        private void ToggleCraftingPlate()
        {
            if (currentCraftingPlate == null)
            {
                // ➤ Spawn the plate
                Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
                Quaternion spawnRotation = Quaternion.identity;

                currentCraftingPlate = Instantiate(craftingPlatePrefab, spawnPosition, spawnRotation);
                Debug.Log("Crafting Plate spawned.");

                // ➤ Assign CraftingManager reference dynamically
                CraftingPlate plateScript = currentCraftingPlate.GetComponent<CraftingPlate>();
                if (plateScript != null)
                {
                    CraftingManager manager = FindObjectOfType<CraftingManager>();
                    if (manager != null)
                    {
                        plateScript.craftingManager = manager;
                        manager.craftingPlate = currentCraftingPlate.transform;
                    }
                    else
                    {
                        Debug.LogError("CraftingManager not found in the scene!");
                    }
                }
                else
                {
                    Debug.LogError("CraftingPlate script not found on the prefab!");
                }
            }
            else
            {
                // ➤ Destroy the plate
                Destroy(currentCraftingPlate);
                currentCraftingPlate = null;
                Debug.Log("Crafting Plate destroyed.");
            }
        }
    }
}
