using UnityEngine;
using TMPro;
using Manager;
using Scriptables;

public class ComputerTerminal : MonoBehaviour
{
    private bool isRivanNearby = false;
    private RivanDataReceiver rivanReceiver;

    [Header("Minigame Message (Do not touch)")]
    public TextMeshProUGUI computerMessage; // Used only for BinaryMiniGameManager

    [Header("New TMP Text for Interaction Prompt")]
    public TextMeshProUGUI interactionPromptText; // ✅ New TMP-only message display

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRivanNearby = true;
            rivanReceiver = other.GetComponent<RivanDataReceiver>();

            if (rivanReceiver == null)
            {
                Debug.LogWarning("RivanDataReceiver not found on player.");
                return;
            }

            // Show interaction text
            if (interactionPromptText != null)
            {
                interactionPromptText.gameObject.SetActive(true);

                if (PlayerHasItemWithPrefabTag("Inverter"))
                    interactionPromptText.text = "Press E to connect the Inverter.";
                else
                    interactionPromptText.text = "You need an Inverter to power this terminal.";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRivanNearby = false;
            rivanReceiver = null;

            if (interactionPromptText != null)
            {
                interactionPromptText.text = "";
                interactionPromptText.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isRivanNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (rivanReceiver != null && PlayerHasItemWithPrefabTag("Inverter"))
            {
                if (interactionPromptText != null)
                {
                    interactionPromptText.text = "";
                    interactionPromptText.gameObject.SetActive(false);
                }

                FindObjectOfType<BinaryMiniGameManager>().StartBinaryGame();
            }
            else
            {
                if (interactionPromptText != null)
                {
                    interactionPromptText.text = "No power supply.";
                }
            }
        }
    }

    private bool PlayerHasItemWithPrefabTag(string requiredTag)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory == null) return false;

        foreach (var slot in inventory.inventorySlots)
        {
            if (slot.item != null && slot.item.prefab != null)
            {
                if (slot.item.prefab.CompareTag(requiredTag))
                    return true;
            }
        }

        return false;
    }
}
