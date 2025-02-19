using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; // Reference to the UI Slider for health
    private Animator animator; // Reference to Animator

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // If healthSlider is not assigned, attempt to find it in child objects
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        // Initialize healthSlider
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " Health: " + currentHealth); // Debug health

        // Update healthSlider UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Play damage animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");

        // Play die animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Destroy the enemy after 2 seconds to allow for the death animation to play
        Destroy(gameObject, 2f);
    }
}
