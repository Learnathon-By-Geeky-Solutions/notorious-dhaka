using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int punchDamage = 10;
    public float activeTime = 0.2f; // Hitbox stays active briefly

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure enemies have the "Enemy" tag
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(punchDamage);
            }
        }
    }

    public void ActivateHitbox()
    {
        gameObject.SetActive(true);
        Invoke(nameof(DeactivateHitbox), activeTime);
    }

    private void DeactivateHitbox()
    {
        gameObject.SetActive(false);
    }
}
