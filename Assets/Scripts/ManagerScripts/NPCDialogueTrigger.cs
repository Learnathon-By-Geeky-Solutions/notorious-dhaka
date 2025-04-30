using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to Dialogue Manager

    private bool hasTalked = false; // Optional: Only trigger once

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTalked)
        {
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue();
                hasTalked = true;
            }
        }
    }
}
