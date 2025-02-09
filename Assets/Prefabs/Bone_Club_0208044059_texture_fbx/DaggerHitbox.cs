using UnityEngine;

public class DaggerHitbox : MonoBehaviour
{
    public int damage = 10; // Damage amount

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the Player has this tag
        {
            PlayerStatus.PlayerHealth playerHealth = other.GetComponent<PlayerStatus.PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit! Health reduced.");
            }
        }
    }
}
