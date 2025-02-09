using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float detectionRange = 5f;  // Range to detect the player
    public float attackRange = 1.5f;   // Attack range
    public float moveSpeed = 2f;
    private Transform player;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
        else if (distance <= detectionRange)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
