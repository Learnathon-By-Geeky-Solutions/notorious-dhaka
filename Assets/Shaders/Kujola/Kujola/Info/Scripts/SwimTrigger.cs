using UnityEngine;

public class SwimTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSwim playerSwim = other.GetComponent<PlayerSwim>();
            if (playerSwim != null)
            {
                playerSwim.StartSwimming();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSwim playerSwim = other.GetComponent<PlayerSwim>();
            if (playerSwim != null)
            {
                playerSwim.StopSwimming(); // Now properly transitions back to Idle
            }
        }
    }
}
