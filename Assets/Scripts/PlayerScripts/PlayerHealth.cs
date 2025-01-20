using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PlayerStatus
{
    public class PlayerHealth : MonoBehaviour
    {
        public float maxHealth = 100f;
        private float currentHealth;

        public TextMeshProUGUI healthText;

        void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthUI();
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            UpdateHealthUI();
            Debug.Log($"Player Health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        void UpdateHealthUI()
        {
            if (healthText != null)
            {
                if (currentHealth > 0)
                {
                    healthText.text = $"Health: {currentHealth}/{maxHealth}";
                }
                else
                {
                    healthText.text = "Player Died";
                }
            }
        }
        void Die()
        {
            Debug.Log("Player is dead.");
            UpdateHealthUI();
            Destroy(gameObject);
        }
    }
}
