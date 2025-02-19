using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true; // Ensure gravity is applied
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
}