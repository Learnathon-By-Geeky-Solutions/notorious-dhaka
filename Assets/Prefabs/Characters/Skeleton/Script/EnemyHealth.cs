using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthSlider;

    private Animator animator;
    private Collider enemyCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider>();

        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("[EnemyHealth] Health Slider is missing on " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("[EnemyHealth] " + gameObject.name + " Health: " + currentHealth);

        UpdateHealthUI();

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("[EnemyHealth] " + gameObject.name + " has died!");
        Debug.Log("[EnemyHealth] Dying object's Tag: " + gameObject.tag);

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (HonourManager.Instance != null)
        {
            HonourManager.Instance.RegisterKill(gameObject.tag);
        }

        Destroy(gameObject, 2f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
