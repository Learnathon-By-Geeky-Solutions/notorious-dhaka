using System.Collections;
using UnityEngine;
using PlayerStatus;
namespace CharacterBehaves
{
    public class FrogBehaviour : MonoBehaviour
    {
        private readonly float roamJumpForce = 5f;
        private readonly float attackJumpForce = 15f;
        private readonly float detectionRange = 10f;
        private readonly float damage = 10f;
        private readonly float jumpCooldown = 2f;
        private readonly float tacklePushForce = 10f;

        private Transform player;
        private Rigidbody rb;
        private bool isJumping = false;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (player == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                SmoothAttackPlayer();
            }
            else
            {
                SmoothRoam();
            }
        }

        void SmoothAttackPlayer()
        {
            if (isJumping) return;

            Vector3 jumpDirection = (player.position - transform.position).normalized;
            jumpDirection.y = 1;
            rb.AddForce(jumpDirection * attackJumpForce, ForceMode.Impulse);

            StartJumpCooldown();
        }

        void SmoothRoam()
        {
            if (isJumping) return;

            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)
            ).normalized;
            randomDirection.y = 1;
            rb.AddForce(randomDirection * roamJumpForce, ForceMode.Impulse);

            StartJumpCooldown();
        }

        void StartJumpCooldown()
        {
            isJumping = true;
            StartCoroutine(JumpCooldownCoroutine());
        }

        IEnumerator JumpCooldownCoroutine()
        {
            yield return new WaitForSeconds(jumpCooldown);
            isJumping = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                    pushDirection.y = 0.5f;
                    playerRb.AddForce(pushDirection * tacklePushForce, ForceMode.Impulse);
                }
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }

        public static void TakeDamage(GameObject target, float damage)
        {
            Debug.Log($"{target.name} took {damage} damage!");
            Destroy(target);
        }
    }
}
