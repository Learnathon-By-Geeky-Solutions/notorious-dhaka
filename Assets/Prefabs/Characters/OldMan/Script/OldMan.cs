using UnityEngine;

public class OldMan : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string[] dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.dialogueLines = dialogue;
            dialogueManager.StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.EndDialogue(); // ✅ Close the dialogue when player exits
        }
    }
}
