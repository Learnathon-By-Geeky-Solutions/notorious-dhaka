using System.Threading.Tasks;
using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int punchDamage = 10;
    public float activeTime = 0.2f; // Active for a short duration per punch

    private Collider hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        hitboxCollider.enabled = false; // Ensure it's off by default
    }

    public async void ActivateHitboxAsync()
    {
        hitboxCollider.enabled = true;

        // Wait asynchronously without blocking the main thread
        await Task.Delay(Mathf.RoundToInt(activeTime * 1000));

        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
            }
        }
    }
}
