using UnityEngine;

public class KickHitbox : MonoBehaviour
{
    public int kickDamage = 15;
    private bool isKicking = false;

    void OnTriggerEnter(Collider other)
    {
        if (isKicking && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(kickDamage);
                Debug.Log("Enemy hit by kick! Health reduced.");
            }
        }
    }

    public void StartKick() { isKicking = true; }
    public void EndKick() { isKicking = false; }
}
