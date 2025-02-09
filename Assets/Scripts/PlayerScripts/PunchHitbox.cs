using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int damage = 10; 
    private Collider punchCollider;

    private void Start()
    {
        punchCollider = GetComponent<Collider>();
        if (punchCollider != null)
        {
            punchCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Punch collided with: {other.name}");

        if (other.CompareTag("Enemy")) 
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy hit! Health reduced.");
            }
        }
    }

    public void EnablePunchHitbox()
    {
        Debug.Log("Punch hitbox enabled");
        punchCollider.enabled = true;
    }

    public void DisablePunchHitbox()
    {
        Debug.Log("Punch hitbox disabled");
        punchCollider.enabled = false;
    }
}
