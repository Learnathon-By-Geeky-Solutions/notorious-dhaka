using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator
    public Transform punchPoint;  // Empty GameObject for punch range
    public float punchRange = 0.5f;  // Radius of punch hitbox
    public LayerMask enemyLayer;  // Layer to detect enemies
    public int punchDamage = 20;  // Damage dealt by punch

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // When F is pressed
        {
            animator.SetTrigger("Punch"); // Play punch animation
            Invoke("Punch", 0.1f); // Delay damage to sync with animation impact
        }
    }

    void Punch()
    {
        // Check if any enemies are within the punch range
        Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(punchDamage); // Damage the enemy
        }
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRange); // Show hitbox in Scene View
    }
}
