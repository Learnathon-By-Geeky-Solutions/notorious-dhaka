using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    public Animator animator;
    public Transform kickPoint;
    public float kickRange = 1.5f;
    public int kickDamage = 20;
    public LayerMask enemyLayer;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (kickPoint == null) Debug.LogError("Kick Point not assigned! Assign it in the Inspector.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Kick();
        }
    }

    private void Kick()
    {
        if (animator == null) return;

        animator.SetTrigger("Kick");

        if (kickPoint == null)
        {
            Debug.LogWarning("Kick Point not set!");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(kickPoint.position, kickPoint.forward, out hit, kickRange, enemyLayer))
        {
            hit.collider.GetComponent<EnemyHealth>()?.TakeDamage(kickDamage);
            Debug.Log("Enemy hit! Dealing " + kickDamage + " damage.");
        }
        else
        {
            Debug.Log("Kick missed! No enemy in range.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (kickPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(kickPoint.position, kickPoint.position + kickPoint.forward * kickRange);
    }
}