using UnityEngine;

public class ComputerTerminal : MonoBehaviour
{
    private bool isRivanNearby = false;
    private RivanDataReceiver rivanReceiver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRivanNearby = true;
            rivanReceiver = other.GetComponent<RivanDataReceiver>();

            if (rivanReceiver == null)
            {
                Debug.LogWarning("RivanDataReceiver component not found on player.");
            }
            else
            {
                Debug.Log("Rivan is near the computer. Press E to process data.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRivanNearby = false;
            rivanReceiver = null;
            Debug.Log("Rivan left the computer terminal.");
        }
    }

    private void Update()
    {
        if (isRivanNearby && Input.GetKeyDown(KeyCode.E) && rivanReceiver != null)
        {
            FindObjectOfType<BinaryMiniGameManager>().StartBinaryGame();
        }
    }
}
