using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }
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
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

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
        if (isDead) return;

        isDead = true;
        Debug.Log(gameObject.name + " has died!");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
