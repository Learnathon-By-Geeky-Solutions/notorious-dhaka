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
        hitboxCollider.enabled = false; 
    }

    public async void ActivateHitboxAsync()
    {
        hitboxCollider.enabled = true;

        
        await Task.Delay(Mathf.RoundToInt(activeTime * 1000));

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
