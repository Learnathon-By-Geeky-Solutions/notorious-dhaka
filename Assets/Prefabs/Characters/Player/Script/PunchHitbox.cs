using System.Threading.Tasks;
using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int punchDamage = 10;
    public float activeTime = 0.2f;
    private Collider hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
        else
            Debug.LogError("Hitbox Collider not found on " + gameObject.name);
    }

    public async void ActivateHitboxAsync()
    {
    
        if (hitboxCollider == null || this == null || gameObject == null)
            return;

        hitboxCollider.enabled = true;

        await Task.Delay(Mathf.RoundToInt(activeTime * 1000));

       
        if (hitboxCollider == null || this == null || gameObject == null)
            return;

        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
            }
        }
    }
}
