using UnityEngine;

public class AxeDamageDealer : MonoBehaviour
{
    public int damageAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Damagable"))
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damageAmount);
                Debug.Log("Axe hit " + other.name + " for " + damageAmount + " damage.");
            }
        }
    }
}
