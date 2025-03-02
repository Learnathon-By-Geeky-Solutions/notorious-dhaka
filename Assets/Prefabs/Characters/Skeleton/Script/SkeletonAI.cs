using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float stopDistance = 2f; // Now adjustable in the Inspector
    public float moveSpeed = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    public int maxHealth = 100;

    private int currentHealth;
    private Animator animator;
    private Rigidbody rb;
    private Transform player;
    private float lastAttackTime = -Mathf.Infinity;
    private float distance;
    private Vector3 moveDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (player == null) return;

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            rb.velocity = Vector3.zero;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
            }
        }
        else if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(moveDirection), 5f * Time.deltaTime);

        animator.SetTrigger("Walk");
    }

    private void AttackPlayer()
    {
        if (player == null) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        DealDamage();
    }

    private void DealDamage()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= stopDistance + 0.5f)
        {
            PlayerStatus.PlayerHealth playerHealth = player.GetComponent<PlayerStatus.PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Player hit by skeleton! Health reduced.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        rb.velocity = Vector3.zero;
    }
}
