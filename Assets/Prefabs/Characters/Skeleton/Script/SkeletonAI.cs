using UnityEngine;
using PlayerStatus;

public class SkeletonAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float stopDistance = 2f;
    public float moveSpeed = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Detection Sound")]
    public AudioSource alertAudio;
    public AudioClip alertClip;
    private bool hasPlayedAlert = false;

    [Header("Environment Layers")]
    public LayerMask waterLayer;

    private Animator animator;
    private Rigidbody rb;
    private Transform player;
    private float lastAttackTime = -Mathf.Infinity;
    private float distance;
    private Vector3 moveDirection;

    private EnemyHealth enemyHealth; // Add reference to EnemyHealth

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;

        if (alertAudio == null)
        {
            alertAudio = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (player == null || enemyHealth == null || enemyHealth.IsDead()) return;

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            rb.velocity = Vector3.zero;

            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");

            if (Time.time - lastAttackTime >= attackCooldown && distance <= stopDistance)
            {
                AttackPlayer();
            }
        }
        else if (distance <= detectionRange)
        {
            if (!hasPlayedAlert)
            {
                PlayAlertSound();
            }

            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector3.zero;

            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");

            hasPlayedAlert = false;
        }
    }

    private void LateUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }
    }

    private void MoveTowardsPlayer()
    {
        moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0;

        if (IsWaterAhead(moveDirection))
        {
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");
            rb.velocity = Vector3.zero;
            return;
        }

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(moveDirection),
            5f * Time.deltaTime
        );

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");
    }

    private bool IsWaterAhead(Vector3 direction)
    {
        Vector3 checkPosition = transform.position + direction.normalized * 1f + Vector3.up * 0.5f;
        return Physics.CheckSphere(checkPosition, 0.5f, waterLayer);
    }

    private void AttackPlayer()
    {
        if (player == null) return;
        if (Vector3.Distance(transform.position, player.position) > stopDistance + 0.2f) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        DealDamage();
    }

    public void DealDamage()
    {
        if (player == null) return;

        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (currentDistance <= stopDistance + 0.5f)
        {
            PlayerStatusManager playerHealth = player.GetComponent<PlayerStatusManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Player hit by skeleton! Health reduced.");
            }
            else
            {
                Debug.LogWarning("PlayerStatusManager not found on player.");
            }
        }
        else
        {
            Debug.Log("Player moved out of range. No damage dealt.");
        }
    }

    private void PlayAlertSound()
    {
        hasPlayedAlert = true;

        if (alertAudio != null && alertClip != null)
        {
            alertAudio.PlayOneShot(alertClip);
            Debug.Log("Skeleton detected the player!");
        }
    }
}
