using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PlayerStatus
{
    public class PlayerStatusManager : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float maxHunger = 100f;
        public float maxThirst = 100f;
        public float maxOxygen = 100f;

        private float currentHealth;
        private float currentHunger;
        private float currentThirst;
        private float currentOxygen;

        public TextMeshProUGUI healthText;
        public TextMeshProUGUI hungerText;
        public TextMeshProUGUI thirstText;
        public TextMeshProUGUI oxygenText;

        void Start()
        {
            currentHealth = maxHealth;
            currentHunger = maxHunger;
            currentThirst = maxThirst;
            currentOxygen = maxOxygen;

            UpdateAllUI();
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void ReduceHunger(float amount)
        {
            currentHunger -= amount;
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
            UpdateHungerUI();
        }

        public void ReduceThirst(float amount)
        {
            currentThirst -= amount;
            currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
            UpdateThirstUI();
        }

        public void ReduceOxygen(float amount)
        {
            currentOxygen -= amount;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
            UpdateOxygenUI();
        }

        void UpdateHealthUI()
        {
            if (healthText != null)
                healthText.text = currentHealth > 0 ? $"{currentHealth}/{maxHealth}" : "Player Died";
        }

        void UpdateHungerUI()
        {
            if (hungerText != null)
                hungerText.text = $"{currentHunger}/{maxHunger}";
        }

        void UpdateThirstUI()
        {
            if (thirstText != null)
                thirstText.text = $"{currentThirst}/{maxThirst}";
        }

        void UpdateOxygenUI()
        {
            if (oxygenText != null)
                oxygenText.text = $"{currentOxygen}/{maxOxygen}";
        }

        void UpdateAllUI()
        {
            UpdateHealthUI();
            UpdateHungerUI();
            UpdateThirstUI();
            UpdateOxygenUI();
        }

        void Die()
        {
            Debug.Log("Player is dead.");
            UpdateHealthUI();
            Destroy(gameObject);
        }

        // ✅ Accessor methods for saving
        public float GetHealth() => currentHealth;
        public float GetHunger() => currentHunger;
        public float GetThirst() => currentThirst;
        public float GetOxygen() => currentOxygen;

        public void SetAllStatus(float health, float hunger, float thirst, float oxygen)
        {
            currentHealth = health;
            currentHunger = hunger;
            currentThirst = thirst;
            currentOxygen = oxygen;
            UpdateAllUI();
        }

        public void HealPlayer(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
        }

    }
}
