using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; // Reference to the UI Slider for health
    private Animator animator; // Reference to Animator
    private bool isDead = false; // Prevent multiple death triggers

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // Try to find health slider if not assigned
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        // Initialize health UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("Health Slider is missing!");
        }
    }

    // **This function is called when the enemy gets punched**
    public void TakeDamage(int damage)
    {
        if (isDead) return; // If already dead, ignore further damage

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        // Update health UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Play damage animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // If health reaches 0, trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death calls

        isDead = true; // Mark as dead
        Debug.Log(gameObject.name + " has died!");

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable enemy collision and attacks after death
        GetComponent<Collider>().enabled = false;

        // Destroy the enemy after 2 seconds to allow death animation to play
        Destroy(gameObject, 2f);
    }
}
