using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthBar;
    public Transform enemy;
    private Vector3 offset = new Vector3(0, 2f, 0);

    void Start()
    {
        if (enemy == null)
        {
            enemy = transform.parent;
        }

        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar Slider is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        if (enemy != null)
        {
            transform.position = enemy.position + offset;
            transform.forward = Camera.main.transform.forward;
        }
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }
}
