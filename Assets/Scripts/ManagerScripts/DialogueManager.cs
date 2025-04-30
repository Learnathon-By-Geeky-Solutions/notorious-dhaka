using UnityEngine;
using TMPro; // ✅ Import TMP namespace

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    private int currentLine = 0;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            currentLine = 0;
            ShowNextDialogue();
        }
    }

    public void ShowNextDialogue()
    {
        if (currentLine < dialogueLines.Length)
        {
            if (dialogueText != null)
                dialogueText.text = dialogueLines[currentLine];

            currentLine++;
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue() // ✅ Modified to public
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}
