using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    public Animator animator;
    public Transform kickPoint;
    public float kickRange = 1.5f;
    public int kickDamage = 20;
    public LayerMask enemyLayer;
    public ParticleSystem kickparticle;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (kickPoint == null) Debug.LogError("Kick Point not assigned! Assign it in the Inspector.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            KickHit();
        }
    }

    public void KickHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(kickPoint.position, kickPoint.forward, out hit, kickRange, enemyLayer))
        {
            hit.collider.GetComponent<EnemyHealth>()?.TakeDamage(kickDamage);
            kickparticle.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (kickPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(kickPoint.position, kickPoint.position + kickPoint.forward * kickRange);
    }
}