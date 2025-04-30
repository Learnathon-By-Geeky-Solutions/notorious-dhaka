using UnityEngine;

public class PlayerStriker : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 2f;
    public int strikeDamage = 25;
    public LayerMask damageLayers;

    public EquipmentHolder equipmentHolder; // ✅ Reference to check if hand is occupied

    void Start()
    {
        if (equipmentHolder == null)
        {
            equipmentHolder = GetComponentInChildren<EquipmentHolder>();
        }
    }

    void Update()
    {
        // ✅ Only attack if equipment is held and Right Click is pressed
        if (Input.GetMouseButtonDown(1)) // 1 = Right Click
        {
            if (equipmentHolder != null && equipmentHolder.equippedObject != null)
            {
                animator.SetTrigger("StrikeTrigger");
            }
            else
            {
                Debug.Log("No item equipped. Cannot strike.");
            }
        }
    }

    // Called by animation event
    public void DealStrikeDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, attackRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Damagable"))
            {
                // Damage logic
                IDamageable target = hit.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(strikeDamage);
                }
                else
                {
                    Debug.Log("Hit but no IDamageable found on: " + hit.name);
                }
            }
        }
    }
}
